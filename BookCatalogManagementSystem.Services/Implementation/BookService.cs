using System.Globalization;
using BookCatalogManagementSystem.Models.Entities;
using BookCatalogManagementSystem.Models.HelpModels;
using BookCatalogManagementSystem.Repository.HelpModel;
using BookCatalogManagementSystem.Repository.Interfaces;
using BookCatalogManagementSystem.Services.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;

namespace BookCatalogManagementSystem.Services.Implementation;

public class BookService : IBookService
{
    private readonly IBaseRepository<Book> _baseRepository;

    public BookService(IBaseRepository<Book> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<Book> CreateBook(Book? book)
    {
        var createdBook = await _baseRepository.Create(book);
        return createdBook;
    }

    public async Task<PageList<Book>> CreateBookFromCsv(IFormFile file)
    {
        var books = new List<Book>();

        await using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
            BadDataFound = null,
            Delimiter = ","
        });
        
        var bookList = csv.GetRecords<Book>().ToList();
        
        foreach (var book in bookList)
        {
            if (string.IsNullOrEmpty(book.Title)) book.Title = "Unknown Title";
            if (string.IsNullOrEmpty(book.Author)) book.Author = "Unknown Author";
            if (string.IsNullOrEmpty(book.Author)) book.Genre = "Unknown Genre";
        }

        books.AddRange(bookList);
        
        // Save the books to the repository
        await _baseRepository.AddRange(books);
        
        return await _baseRepository
            .GetSearchPaged(1, 10, null, "asc", null);
    }

    public async Task<Book> UpdateBook(Book? book)
    {
        var createdBook = await _baseRepository.Update(book);
        return createdBook;
    }
    
    public async Task<bool> DeleteBook(int bookId)
    {
        var bookById = await _baseRepository.FindById(bookId);
        if (bookById != null)
        {
            await _baseRepository.RemoveAsync(bookById);
            return true;
        }

        return false;
    }

    public async Task<PageList<Book?>> GetBooks(BookPageParameters bookParam)
    {
        PageList<Book> responseFromDb;
        if (string.IsNullOrEmpty(bookParam.SearchValue))
        {
            responseFromDb = await _baseRepository
                .GetPaged(bookParam.PageNumber, bookParam.PageSize,
                    bookParam.SortField, bookParam.SortDirection);
        }
        else
        {
            responseFromDb = await _baseRepository
                .GetSearchPaged(bookParam.PageNumber, bookParam.PageSize,
                    bookParam.SortField, bookParam.SortDirection, bookParam.SearchValue);
        }
        
        return responseFromDb;
    }
}