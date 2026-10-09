using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Application.Purchase;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Infrastructure.DataBase;


namespace Tecnyfarma.Server.Purchase.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddPurchaseDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PurchasesDatabase") ?? "Data Source=purchases.sqlite";
        services.AddDbContext<DataBase.DbContext>(options => options
            .UseSqlite(connectionString)
            .ReplaceService<IHistoryRepository, NoLockSqliteHistoryRepository>());
        
        services.AddScoped<CreatePurchaseUseCase>();
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<ProductRepository, SqliteProductRepository>();
        services.AddScoped<UserRepository, SqliteUserRepository>();
        services.AddScoped<PurchaseRepository, SqlitePurchaseRepository>();
        
        var options = new DbContextOptionsBuilder<DataBase.DbContext>()
            .UseSqlite(connectionString)
            .Options;
        using var dbContext = new DataBase.DbContext(options);
        dbContext.Database.Migrate();
        
        return services;

    }
}