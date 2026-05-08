using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.Login;
using Tecnyfarma.Server.User.Application.Register;

namespace Tecnyfarma.Server.User.Infrastructure;

public static class Dependencies
{
    public static void AddUserDependencies(this IServiceCollection services)
    {
        services.AddScoped<UserRepository, SqlUserRepository>();
        services.AddScoped<RegisterUseCase>();
        services.AddScoped<LoginUseCase>();
    }
}