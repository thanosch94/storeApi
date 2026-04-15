namespace StoreApi.Data.Dto
{
    public class ListOptionsDto
    {
        public bool PagingEnabled { get; set; }
        public string? SearchText { get; set; }
        public int? PageNumber { get; set; }
        public int? ItemsPerPage { get; set; }

        public Guid? CategoryId { get; set; }

        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }


    }
}
