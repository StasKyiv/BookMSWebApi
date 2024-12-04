using BookCatalogManagementSystem.Models.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BookCatalogManagementSystem.Services.SignalR;

public class BooksHub : Hub
{
    public async Task BroadcastBook(Book book)
    {
        await Clients.All.SendAsync("ReceiveBook", book);
    }
}