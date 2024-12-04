namespace BookCatalogManagementSystem.Models.HelpModels;

public abstract class QueryParameters
{
    const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    public string? SortField { get; set; }
    public string? SortDirection { get; set; }
    private int _pageSize = 10;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}