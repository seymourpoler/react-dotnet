using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddDbContext<DbContext>(options => options
            .UseSqlite(connectionString)
            .ReplaceService<IHistoryRepository, NoLockSqliteHistoryRepository>());
        
        services.AddScoped<FindProductsUseCase>();
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<ProductRepository, SqliteProductRepository>();
        services.AddScoped<UserRepository, SqliteUserRepository>();

        var options = new DbContextOptionsBuilder<DbContext>()
            .UseSqlite(connectionString)
            .ReplaceService<IHistoryRepository, NoLockSqliteHistoryRepository>()
            .Options;
        using var dbContext = new DbContext(options);
        dbContext.Database.Migrate();

        return services;
    }
}