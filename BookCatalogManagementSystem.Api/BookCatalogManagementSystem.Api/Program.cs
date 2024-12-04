using System.Text.Json;
using AspNetCoreRateLimit;
using AutoMapper;
using BookCatalogManagementSystem.Models.DTOs;
using BookCatalogManagementSystem.Models.Entities;
using BookCatalogManagementSystem.Models.HelpModels;
using BookCatalogManagementSystem.Repository;
using BookCatalogManagementSystem.Services.Interfaces;
using BookCatalogManagementSystem.Services.SignalR;
using BookCatalogManagementSystemAPI.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using IFormFile = Microsoft.AspNetCore.Http.IFormFile;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5054", "http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("DbInMemory"));

builder.Services.InitializeServices();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddInMemoryRateLimiting();

var app = builder.Build();

app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}
else
{
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseIpRateLimiting();
app.MapHub<BooksHub>("/hubs/books");

app.MapGet("/", async (
        [FromServices] IBookService bookService,
        [FromServices] IMapper mapper,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] string sortField = "id",
        [FromQuery] string sortDirection = "asc",
        [FromQuery] string? searchValue = null) =>
    {
        try
        {
            var bookParam = new BookPageParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortField = sortField,
                SortDirection = sortDirection,
                SearchValue = searchValue
            };

            var pagedBooks = await bookService.GetBooks(bookParam);
            var pageBookDto = mapper.Map<PageListDto<BookDto>>(pagedBooks);
            
            var json = JsonSerializer.Serialize(pageBookDto);
            return Results.Ok(json);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    })
    .WithName("GetBooks")
    .WithOpenApi();

app.MapPost("/", async (
        [FromServices] IBookService bookService,
        [FromServices] IMapper mapper,
        BookDto bookDto) =>
    {
        try
        {
            var book = mapper.Map<Book>(bookDto);
            var pagedBooks = await bookService.CreateBook(book);
            var json = JsonSerializer.Serialize(pagedBooks);
            return Results.Ok(json);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("CreateBook")
    .WithOpenApi();


app.MapPost("/add-with-file", [IgnoreAntiforgeryToken] async (
        [FromServices] IBookService bookService,
        [FromServices] IMapper mapper,
        [FromServices] IHubContext<BooksHub> hubContext,
        IFormFile file) =>
    {
        try
        {
            var response = await bookService.CreateBookFromCsv(file);
            
            var responseBookDto = mapper.Map<PageListDto<BookDto>>(response);
            var json = JsonSerializer.Serialize(responseBookDto);
            
            await hubContext.Clients.All.SendAsync("ReceiveBook", json);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("CreateBookFromCsv")
    .DisableAntiforgery();

app.MapPut("/", async (
        [FromServices] IBookService bookService,
        [FromServices] IMapper mapper,
        BookDto bookDto) =>
    {
        try
        {
            var book = mapper.Map<Book>(bookDto);
            var pagedBooks = await bookService.UpdateBook(book);
            var json = JsonSerializer.Serialize(pagedBooks);
            return Results.Ok(json);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("UpdateBook")
    .WithOpenApi();

app.MapDelete("/", async (
        [FromServices] IBookService bookService,
        [FromServices] IMapper mapper,
        int bookId) =>
    {
        try
        {
            var deleteBook = await bookService.DeleteBook(bookId);
            return deleteBook ? Results.Ok() : Results.BadRequest();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("DeleteBook")
    .WithOpenApi();

app.Run();