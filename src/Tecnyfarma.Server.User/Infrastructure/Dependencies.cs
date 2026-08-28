using Microsoft.Extensions.DependencyInjection;
using Tecnyfarma.Server.User.Application;

namespace Tecnyfarma.Server.User.Infrastructure;

public static class Dependencies
{
    public static void AddUserDependencies(this IServiceCollection services)
    {
        services.AddScoped<UserRepository, SqlUserRepository>();
        services.AddScoped<Application.Register.UseCase>();
        services.AddScoped<Application.LogIn.UseCase>();
    }
}