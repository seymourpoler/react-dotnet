using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tecnyfarma.Server.Product.Application;
using Tecnyfarma.Server.Product.Infrastructure.DataBase;

namespace Tecnyfarma.Server.Product.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddProductDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductsDatabase") ?? "Data Source=products.db";
        // services.AddDbContext<ProductsDbContext>(options => options.UseSqlite(connectionString));
        
        services.AddScoped<UseCase>();
        services.AddScoped<Repository, SqliteRepository>();
        return services;
    }
}