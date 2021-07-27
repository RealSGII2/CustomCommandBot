namespace CustomCommandBot.Server.Models.Pagination
{
    public class PaginationOptions
    {
        public int CurrentPage { get; init;}
        public int TotalPages { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; set; }

        public bool CanGoForward => CurrentPage < TotalPages;
        public bool CanGoBack => CurrentPage > 0;
    }
}
