using Microsoft.EntityFrameworkCore;
using StoreApi.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace StoreApi.Data.Dto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public int? SerialNumber { get; set; }
        public string? AffiliateId { get; set; }

        public string? Sku { get; set; }
        public string Name { get; set; }
        public string? Barcode { get; set; }

        public string? Description { get; set; }
        public string? AffiliateUrl { get; set; }

        public Guid? AffiliateProgramId { get; set; }
        public bool? IsInStock { get; set; }
        public Guid? BrandId { get; set; }

        public string? FeatureImageUrl { get; set; }

        public decimal? Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool IsActive { get;  set; }
    }
}
