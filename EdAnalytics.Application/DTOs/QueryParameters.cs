namespace EdAnalytics.Application.DTOs
{
    public class QueryParameters
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int Page { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public string? OrderBy { get; set; }
        public bool Descending { get; set; } = false;
        public string? Search { get; set; }
        public string? Area { get; set; }
    }
}
