using System.Linq.Dynamic.Core;
using BookCatalogManagementSystem.Models.Entities;
using BookCatalogManagementSystem.Repository.HelpModel;
using BookCatalogManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogManagementSystem.Repository.Implementation;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = db.Set<T>();
    }

    public async Task<T?> FindById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> Create(T item)
    {
        var result = await _dbSet.AddAsync(item);
        await SaveChangesAsync();
        return result.Entity;
    }

    public async Task AddRange(List<T> item)
    {
        await _dbSet.AddRangeAsync(item);
        await SaveChangesAsync();
    }

    public async Task<T?> Update(T item)
    {
        _dbSet.Update(item);
        await SaveChangesAsync();
        return item;
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<PageList<T>> GetPaged(
        int pageNumber, int pageSize, string? sortField, string? sortDirection)
    {
        var source = _dbSet.AsNoTracking();

        var query = source.OrderBy($"{sortField} {sortDirection}");

        return await PageList<T>.ToPagedList(query, pageNumber, pageSize);
    }

    public async Task<PageList<T>> GetSearchPaged(
        int pageNumber,
        int pageSize,
        string? sortField,
        string? sortDirection,
        string? searchValue)
    {
        var source = _dbSet.AsNoTracking();

        if (!string.IsNullOrEmpty(searchValue))
        {
            var searchPattern = $"%{searchValue}%";
            
            if (typeof(T) == typeof(Book))
            {
                source = source.Where(x =>
                    EF.Functions.Like(((Book)(object)x).Title, searchPattern) ||
                    EF.Functions.Like(((Book)(object)x).Author, searchPattern) ||
                    EF.Functions.Like(((Book)(object)x).Genre, searchPattern));
            }
        }
        
        if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortDirection))
        {
            source = source.OrderBy($"{sortField} {sortDirection}");
        }
        
        return await PageList<T>.ToPagedList(source, pageNumber, pageSize);
    }


    public async Task RemoveAsync(T item)
    {
        _dbSet.Remove(item);
        await SaveChangesAsync();
    }
}