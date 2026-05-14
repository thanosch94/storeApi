using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using StoreApi.Data;
using StoreApi.Data.Enums;
using StoreApi.Data.Models;
using StoreApi.Services;
using System.Net;
using System;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]

    public class BaseController : Controller
    {

        private ApplicationDbContext _context;
        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public BaseController()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            //if (userAgent == "node"){
            //    return;
            //};
            var referer = HttpContext.Request.Headers["Referer"].ToString();

            var platform = HttpContext.Request.Headers["Sec-CH-UA-Platform"]
                .ToString()
                .Replace("\"", "");

            var consentId = HttpContext.Request.Cookies["afh_consent_id"];
            var hasConsent = !string.IsNullOrWhiteSpace(consentId);

            var device = GetUserDevice();

            var id = context.RouteData.Values["id"]?.ToString();
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            Guid? entityId = null;

            if (Guid.TryParse(id, out var parsedId))
            {
                entityId = parsedId;
            }
            var location = GetLocationFromIp(ip);
            var trafficSource = DetectTrafficSource(HttpContext.Request);
            var analytics = new Analytics
            {
                Id = Guid.NewGuid(),
                PageId = entityId??Guid.Empty,
                Controller = controller,
                Action = action,
                Source = trafficSource,
                Device = device,
                Referer = referer,
                Platform = platform,
                AffiliateUrlClick = null,
                CountryCode = location.CountryCode,
                Country = location.CountryName,
                City = location.City,
                DateAdded = DateTime.UtcNow
            };

            if (hasConsent)
            {
                var sessionHash = HttpContext.Request.Cookies["analytics_session_id"];

                if (string.IsNullOrWhiteSpace(sessionHash))
                {
                    sessionHash = Guid.NewGuid().ToString("N");
                }

                analytics.SessionHash = sessionHash;
                analytics.VisitorHash = CreateStableVisitorHash(ip, userAgent);
                analytics.TrackingMode = AnalyicsTrackingModeEnum.Consent;

                Response.Cookies.Append("analytics_session_id", sessionHash, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    IsEssential = false,
                    Path = "/"
                });
            }
            else
            {
                analytics.SessionHash = CreateAnonymousSessionHash(ip, userAgent);
                analytics.VisitorHash = null;
                analytics.TrackingMode = AnalyicsTrackingModeEnum.Basic;
            }

            using (var dbContext = new ApplicationDbContext(AppBase.ConnectionString))
            {
                dbContext.Analytics.Add(analytics);
                dbContext.SaveChanges();
            }

            base.OnActionExecuting(context);
        }
        private (string? CountryCode, string? CountryName, string? City) GetLocationFromIp(string? ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return (null, null, null);

            if (!IPAddress.TryParse(ip, out var ipAddress))
                return (null, null, null);

            if (IPAddress.IsLoopback(ipAddress))
                return ("LOCAL", "Localhost", "Localhost");

            var dbPath = Path.Combine(AppContext.BaseDirectory, "App_Data", "GeoLite2-City.mmdb");

            try
            {
                using var reader = new DatabaseReader(dbPath);
                var city = reader.City(ipAddress);

                return (
                    city.Country.IsoCode,
                    city.Country.Name,
                    city.City.Name
                );
            }
            catch (AddressNotFoundException)
            {
                return (null, null, null);
            }
        }

        private TrafficSourceEnum DetectTrafficSource(HttpRequest request)
        {
            var referer = request.Headers["Referer"].ToString();

            if (string.IsNullOrWhiteSpace(referer))
            {
                return TrafficSourceEnum.Direct;
            }

            if (!Uri.TryCreate(referer, UriKind.Absolute, out var refererUri))
            {
                return TrafficSourceEnum.Direct;
            }

            var host = refererUri.Host.ToLowerInvariant();
            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var websiteUrl = configuration["WebsiteUrl"];

            var ownDomains = new[]
            {
                websiteUrl,
                "www." +websiteUrl,
                "localhost"
            };

            if (ownDomains.Contains(host))
                return TrafficSourceEnum.Internal;

            if (host.Contains("google."))
                return TrafficSourceEnum.Google;

            if (host.Contains("bing."))
                return TrafficSourceEnum.Bing;

            if (host.Contains("facebook.") || host.Contains("fb."))
                return TrafficSourceEnum.Facebook;

            if (host.Contains("instagram."))
                return TrafficSourceEnum.Instagram;

            if (host.Contains("youtube."))
                return TrafficSourceEnum.Youtube;

            if (host.Contains("tiktok."))
                return TrafficSourceEnum.Tiktok;

            return TrafficSourceEnum.Referral;
        }

        private DeviceEnum GetUserDevice()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var platform = HttpContext.Request.Headers["Sec-CH-UA-Platform"]
                .ToString()
            .Replace("\"", "");

            var mobile = HttpContext.Request.Headers["Sec-CH-UA-Mobile"].ToString();

            if (mobile == "?1")
                return DeviceEnum.Mobile;

            if (platform == "Android" || platform == "iOS")
                return DeviceEnum.Mobile;

            if (userAgent.Contains("Mobi", StringComparison.OrdinalIgnoreCase) ||
                userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase) ||
                userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase))
            {
                return DeviceEnum.Mobile;
            }

            if (userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase) ||
                userAgent.Contains("Tablet", StringComparison.OrdinalIgnoreCase))
            {
                return DeviceEnum.Tablet;
            }

            return DeviceEnum.Desktop;
        }

        private string? CreateAnonymousSessionHash(string? ip, string? userAgent)
        {
            if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(userAgent))
                return null;

            var salt = GetAnalyticsSalt();

            var now = DateTimeOffset.UtcNow;
            var bucketMinute = now.Minute < 30 ? 0 : 30;

            var bucket = new DateTimeOffset(
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                bucketMinute,
                0,
                TimeSpan.Zero
            ).ToString("yyyy-MM-dd-HH-mm");

            return CreateHash($"{ip}|{userAgent}|{bucket}|{salt}");
        }

        private string? CreateStableVisitorHash(string? ip, string? userAgent)
        {
            if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(userAgent))
                return null;

            var salt = GetAnalyticsSalt();

            return CreateHash($"{ip}|{userAgent}|{salt}");
        }

        private string GetAnalyticsSalt()
        {
            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var salt = configuration["Analytics:HashSalt"];

            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new InvalidOperationException("Analytics:HashSalt is missing from configuration.");
            }

            return salt;
        }

        private string CreateHash(string input)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();

            var bytes = sha.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(input)
            );

            return Convert.ToHexString(bytes);
        }

        public Guid GetCompanyFromHeader()
        {
            if (Request.Headers.TryGetValue("CompanyId", out var id))
            {
                // Use the header value here
                //var company = new { Header = companyId.ToString() };
                var companyId = id.ToString();
                return Guid.Parse(companyId);
            }
            else
            {
                return Guid.Empty;
            }

        }

        public int GetUserTimeZone()
        {
            if (Request.Headers.TryGetValue("TimeZoneOffset", out var offset))//Offset received in minutes
            {
                int.TryParse(offset.ToString(), out var tz);
                // Use the header value here
                int timeZoneOffset = tz;
                return timeZoneOffset;
            }
            else
            {
                return 0;
            }

        }

        public async Task<User> GetActionUser()
        {
            string userData = User.Claims.FirstOrDefault()?.Value;
            var actionUser = new User();

            if (userData != null)
            {
                JsonConvert.PopulateObject(userData, actionUser);

            }
            return actionUser;
        }

        protected void LogMessage(string message, LogTypeEnum logType, LogOriginEnum logOrigin, Guid? userId)
        {
           using(var context = new ApplicationDbContext(AppBase.ConnectionString))
            {
                LogService.CreateLog(message, logType, logOrigin, userId, context);

            }


        }
    }
}
