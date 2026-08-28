using Microsoft.Extensions.DependencyInjection;
using Tecnyfarma.Server.User.Application;
using Microsoft.Extensions.Configuration;
using Tecnyfarma.Server.User.Infrastructure.DataBase;

namespace Tecnyfarma.Server.User.Infrastructure;

public static class Dependencies
{
    public static void AddUserDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UsersDatabase") 
                               ?? "Data Source=users.db";
        services.AddScoped<UserRepository, SqlUserRepository>();
        services.AddScoped<Application.Register.UseCase>();
        services.AddScoped<Application.LogIn.UseCase>();
    }
}