using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Application.Purchase;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Infrastructure.DataBase;
using DbContext = Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext;

namespace Tecnyfarma.Server.Purchase.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddPurchaseDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PurchasesDatabase") ?? "Data Source=purchases.sqlite";
        services.AddDbContext<DbContext>(options => options
            .UseSqlite(connectionString)
            .ReplaceService<IHistoryRepository, NoLockSqliteHistoryRepository>());
        
        services.AddScoped<CreatePurchaseUseCase>();
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<ProductRepository, SqliteProductRepository>();
        services.AddScoped<UserRepository, SqliteUserRepository>();
        
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseSqlite(connectionString)
            .Options;
        using var dbContext = new DbContext(options);
        dbContext.Database.Migrate();
        
        return services;

    }
}