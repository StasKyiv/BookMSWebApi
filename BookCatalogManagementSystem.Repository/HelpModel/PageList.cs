using Microsoft.EntityFrameworkCore;

namespace BookCatalogManagementSystem.Repository.HelpModel;

public class PageList<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasPropertyInNextPage { get; set; }
    public bool HasNext => CurrentPage < TotalPages;
    public List<T> Items { get; set; }
    public PageList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = new List<T>();
        Items.AddRange(items);
    }
    public static async Task<PageList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var result = new PageList<T>(items, count, pageNumber, pageSize);
        return result;
    }
}