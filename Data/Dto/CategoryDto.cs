namespace StoreApi.Data.Dto
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public string? ImagePath { get; set; }
        public string? Description { get; set; }
        public string? SeoTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}
