using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tecnyfarma.Server.User.Application;
using Microsoft.Extensions.Configuration;
using Tecnyfarma.Server.User.Infrastructure.DataBase;

namespace Tecnyfarma.Server.User.Infrastructure;

public static class Dependencies
{
    public static void AddUserDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UsersDatabase") ?? "Data Source=users.db";
        services.AddDbContext<UsersDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<UserRepository, SqliteUserRepository>();
        services.AddScoped<Application.Register.UseCase>();
        services.AddScoped<Application.LogIn.UseCase>();
    }
}