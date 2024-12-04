using BookCatalogManagementSystem.Repository.HelpModel;

namespace BookCatalogManagementSystem.Repository.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> FindById(int id);
    Task<T?> Create(T? item);
    Task<T?> Update(T? item);
    Task RemoveAsync(T item);
    Task AddRange(List<T> item);
    Task<PageList<T>> GetPaged(
        int pageNumber, int pageSize, string? sortField = "Id", string? sortDirection = "asc");

    Task<PageList<T>> GetSearchPaged(
        int pageNumber,
        int pageSize,
        string? sortField,
        string? sortDirection,
        string? searchValue);
}