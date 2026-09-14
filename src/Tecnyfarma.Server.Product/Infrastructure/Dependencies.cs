using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tecnyfarma.Server.Product.Application;
using Tecnyfarma.Server.Product.Application.Product;
using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.Product.Infrastructure.DataBase;
using DbContext = Tecnyfarma.Server.Product.Infrastructure.DataBase.DbContext;

namespace Tecnyfarma.Server.Product.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddProductDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductsDatabase") ?? "Data Source=products.db";
        services.AddDbContext<DbContext>(options => options.UseSqlite(connectionString));
        
        services.AddScoped<FindProductsUseCase>();
        services.AddScoped<ProductRepository, SqliteProductRepository>();
        services.AddScoped<UserRepository, SqliteUserRepository>();
        return services;
    }

    public static void MigrateProductDatabase(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        dbContext.Database.Migrate();
    }
}