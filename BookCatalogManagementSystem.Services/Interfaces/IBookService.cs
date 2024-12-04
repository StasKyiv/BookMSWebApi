using BookCatalogManagementSystem.Models.Entities;
using BookCatalogManagementSystem.Models.HelpModels;
using BookCatalogManagementSystem.Repository.HelpModel;
using Microsoft.AspNetCore.Http;

namespace BookCatalogManagementSystem.Services.Interfaces;

public interface IBookService
{
    Task<PageList<Book?>> GetBooks(BookPageParameters bookParam);
    Task<Book> CreateBook(Book? book);
    Task<Book> UpdateBook(Book? book);
    Task<bool> DeleteBook(int bookId);
    Task<PageList<Book>> CreateBookFromCsv(IFormFile file);
}