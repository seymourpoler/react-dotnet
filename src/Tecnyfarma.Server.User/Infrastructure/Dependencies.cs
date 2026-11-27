using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.Purchase;
using Tecnyfarma.Server.User.Application.Purchase.Create;
using Tecnyfarma.Server.User.Application.Purchase.Find;
using Tecnyfarma.Server.User.Infrastructure.DataBase;
using DbContext = Tecnyfarma.Server.User.Infrastructure.DataBase.DbContext;
using UseCase = Tecnyfarma.Server.User.Application.Purchase.Find.UseCase;

namespace Tecnyfarma.Server.User.Infrastructure;

public static class Dependencies
{
    public static void AddUserDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UsersDatabase") ?? "Data Source=users.sqlite";
        services.AddDbContext<DbContext>(options => options
            .UseSqlite(connectionString)
            .ReplaceService<IHistoryRepository, NoLockSqliteHistoryRepository>());
        services.AddScoped<UserRepository, SqliteUserRepository>();
        services.AddScoped<Application.LogIn.UseCase>();
        services.AddScoped<Application.Register.UseCase>();
        services.AddScoped<Application.Purchase.Create.UseCase>();
        services.AddScoped<UseCase>();
        services.AddScoped<Tecnyfarma.Server.User.Application.Purchase.Repository, Tecnyfarma.Server.User.Infrastructure.DataBase.SqlitePurchaseRepository>();

        var options = new DbContextOptionsBuilder<DbContext>().UseSqlite(connectionString).Options;
        using var dbContext = new DbContext(options);
        dbContext.Database.Migrate();
    }
}