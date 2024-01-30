namespace BlazorServerFrontend.DTOs
{
    public class PagedList<T>
    {
        public List<T> PagedItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage { get; set; }

    }
}
