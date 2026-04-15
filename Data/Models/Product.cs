using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreApi.Data.Models
{
    [Table("store_products")]
    public class Product : BaseModel
    {
        public Product()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string? AffiliateId { get; set; }

        public string? Sku { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Barcode { get; set; }

        public string? Description { get; set; }
        public string? AffiliateUrl { get; set; }

        public Guid? AffiliateProgramId { get; set; }

        public AffiliateProgram? AffiliateProgram { get; set; }

        public bool? IsInStock { get; set; }
        public Guid? BrandId { get; set; }

        public Brand? Brand { get; set; }

        public string? FeatureImageUrl { get; set; }


        [Precision(18, 2)]
        public virtual decimal? Price { get; set; }

        [Precision(18, 2)]
        public virtual decimal? DiscountPrice { get; set; }


    }
}
