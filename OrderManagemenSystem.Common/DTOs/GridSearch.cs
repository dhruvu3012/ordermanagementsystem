namespace OrderManagemenSystem.Common.DTOs
{
    public class GridSearchRequest
    {
        public string? SearchText {  get; set; }
        public string? SortColumn { get; set; } 
        public string? SortType { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }

    public class GridSearchResult : GridSearchRequest
    {
        public int TotalCount { get; set; }
        public object? Data { get; set; }

        public GridSearchResult(GridSearchRequest searchRequest, object? data,int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
            SearchText = searchRequest.SearchText;
            SortColumn = searchRequest.SortColumn;
            SortType = searchRequest.SortType;
            PageNumber = searchRequest.PageNumber;
            PageSize = searchRequest.PageSize;
        }
    }
}
