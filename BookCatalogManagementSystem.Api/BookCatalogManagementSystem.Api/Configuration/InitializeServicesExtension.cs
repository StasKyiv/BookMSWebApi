using AspNetCoreRateLimit;
using BookCatalogManagementSystem.Repository.Implementation;
using BookCatalogManagementSystem.Repository.Interfaces;
using BookCatalogManagementSystem.Services.Implementation;
using BookCatalogManagementSystem.Services.Interfaces;

namespace BookCatalogManagementSystemAPI.Configuration;

public static class InitializeServicesExtension
{
    public static void InitializeServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IBookService, BookService>();
    }
}