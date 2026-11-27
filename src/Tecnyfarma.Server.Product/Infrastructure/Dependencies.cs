using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tecnyfarma.Server.Product.Application.Product;
using Tecnyfarma.Server.Product.Application.Product.Create;
using Tecnyfarma.Server.Product.Application.Product.Find;
using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.Product.Infrastructure.DataBase;
using UseCase = Tecnyfarma.Server.Product.Application.Product.Find.UseCase;

namespace Tecnyfarma.Server.Product.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddProductDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductsDatabase") ?? "Data Source=products.sqlite";
        services.AddDbContext<DataBase.DbContext>(options => options
            .UseSqlite(connectionString)
            .ReplaceService<IHistoryRepository, NoLockSqliteHistoryRepository>());
        
        services.AddScoped<UseCase>();
        services.AddScoped<Application.User.UseCase>();
        services.AddScoped<Application.Product.Create.UseCase>();
        services.AddScoped<Tecnyfarma.Server.Product.Application.Product.Repository, SqliteRepository>();
        services.AddScoped<Tecnyfarma.Server.Product.Application.User.Repository, SqliteUserRepository>();

        var options = new DbContextOptionsBuilder<DataBase.DbContext>()
            .UseSqlite(connectionString)
            .ReplaceService<IHistoryRepository, NoLockSqliteHistoryRepository>()
            .Options;
        using var dbContext = new DataBase.DbContext(options);
        dbContext.Database.Migrate();

        return services;
    }
}