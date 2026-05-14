using StoreApi.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.Data.Models
{

    [Table("store_analytics")]
    public class Analytics:BaseModel
    {
        public Analytics()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid PageId { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public AnalyicsTrackingModeEnum TrackingMode { get; set; }
        public DeviceEnum Device { get; set; }
        public string? Referer { get; set; }
        public TrafficSourceEnum? Source { get; set; }
        public string? Platform { get; set; }        
        public string? CountryCode { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string SessionHash { get; set; }
        public string? VisitorHash { get; set; }
        public string? AffiliateUrlClick { get; set; }
    }
}
