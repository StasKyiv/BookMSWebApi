namespace BookCatalogManagementSystem.Models.DTOs;

public class PageListDto<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasPropertyInNextPage { get; set; }
    public bool HasNext => CurrentPage < TotalPages;
    public List<T> Items { get; set; }
}