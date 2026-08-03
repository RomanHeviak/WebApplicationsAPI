namespace WebApplicationAPI.Dtos.Common
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}
