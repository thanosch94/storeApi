using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.Data.Models
{

    [Table("store_affiliate_programs")]
    public class AffiliateProgram:BaseModel
    {
        public AffiliateProgram()
        {
              Id=Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();

    }
}
