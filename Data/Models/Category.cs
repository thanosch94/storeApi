using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.Data.Models
{
    [Table("store_categories")]
    public class Category : BaseModel
    {
        public Category()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? ImagePath { get; set; }
        public string? Description { get; set; }
        public string? SeoTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}
