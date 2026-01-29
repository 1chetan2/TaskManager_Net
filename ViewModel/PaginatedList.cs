namespace JwtApi.ViewModel
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }

        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public string SortColumn { get; set; }
        public string SortOrder { get; set; }

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public PaginatedList(List<T> items, int count, int page, int pageSize, string sortColumn, string sortOrder)
        {
            Items = items;
            Page = page;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            SortColumn = sortColumn;
            SortOrder = sortOrder;
        }
    }
}
