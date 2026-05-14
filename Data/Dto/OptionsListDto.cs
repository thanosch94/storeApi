namespace StoreApi.Data.Dto
{
    public class OptionsListDto<T>
    {
        public List<T> List { get; set; }
        public int TotalPages { get; set; }
    }
}
