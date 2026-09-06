using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Infrastructure.DataBase;
using DbContext = Tecnyfarma.Server.User.Infrastructure.DataBase.DbContext;

namespace Tecnyfarma.Server.User.Infrastructure;

public static class Dependencies
{
    public static void AddUserDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UsersDatabase") ?? "Data Source=users.db";
        services.AddDbContext<DbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<Repository, SqliteRepository>();
        services.AddScoped<Application.LogIn.UseCase>();
        services.AddScoped<Application.Register.UseCase>();
    }
}