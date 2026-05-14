using Microsoft.AspNetCore.Mvc;
using StoreApi.Interfaces;

namespace StoreApi.Controllers
{
    public class ConsentController : BaseController
    {
        private IConsentProcessor _consentProcessor;

        public ConsentController(IConsentProcessor consentProcessor)
        {
            _consentProcessor = consentProcessor;
        }

        [HttpGet("accept-all")]
        public IActionResult AcceptAll()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();

            var consentCookieId = _consentProcessor.CreateConsentCookie(ip, userAgent);

            Response.Cookies.Append("afh_consent_id", consentCookieId.ToString(), new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMonths(12),
                IsEssential = true
            });

            Response.Cookies.Append("afh_consent_level", "all", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMonths(12),
                IsEssential = true
            });

            return Ok();
        }
    }
}
