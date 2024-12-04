using BookCatalogManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogManagementSystem.Repository;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Book> Book { get; set; }
}