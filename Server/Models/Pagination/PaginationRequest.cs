namespace CustomCommandBot.Server.Models.Pagination
{
    public class PaginationRequest
    {
        private int _pageSize;

        public int MaxSize { get; } = 100;

        public int Page { get; set; } = 0;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = _pageSize > MaxSize ? MaxSize : value;
        }
    }
}
