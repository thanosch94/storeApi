using Newtonsoft.Json;
using StoreApi.Data;
using StoreApi.Data.Enums;
using StoreApi.Interfaces;
using StoreApi.Services;

namespace StoreApi.Processors
{
    public class ConsentProcessor: IConsentProcessor
    {
        private ApplicationDbContext _context;
        public ConsentProcessor(ApplicationDbContext context)
        {
            _context = context;
        }


        public Guid CreateConsentCookie(string ip, string userAgent)
        {
            var consentCookieId = Guid.NewGuid();

            var log = new
            {
                Id = Guid.NewGuid(),
                ConsentCookieId = consentCookieId,
                AnalyticsConsent = true,
                MarketingConsent = true,
                NecessaryConsent = true,
                Action = "accepted_all",
                PolicyVersion = "2026-05-01",
                CookieBannerVersion = "v1",
                ConsentTextSnapshot = "We use necessary cookies and, with your consent, analytics and marketing cookies...",
                IpHash = CreateAuditHash(ip),
                UserAgentHash = CreateAuditHash(userAgent),
                CreatedAt = DateTime.UtcNow
            };

            LogService.CreateLog(JsonConvert.SerializeObject(log), LogTypeEnum.Consent, LogOriginEnum.StoreApp, Guid.Empty, _context);


            return consentCookieId;
        }


        private string? CreateAuditHash(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var salt = "audit-secret-from-config";

            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = sha.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes($"{value}|{salt}")
            );

            return Convert.ToHexString(bytes);
        }
    }
}
