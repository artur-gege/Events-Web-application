namespace ModsenAPI.Infrastructure.Pagination
{
    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int NextPage { get; set; }
        public int PrevPage { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Items { get; set; }
    }

}
