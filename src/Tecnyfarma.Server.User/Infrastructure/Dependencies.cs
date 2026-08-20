using Microsoft.Extensions.DependencyInjection;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.SigIn;
using Tecnyfarma.Server.User.Application.SignUp;

namespace Tecnyfarma.Server.User.Infrastructure;

public static class Dependencies
{
    public static void AddUserDependencies(this IServiceCollection services)
    {
        services.AddScoped<UserRepository, SqlUserRepository>();
        services.AddScoped<SignInUseCase>();
        services.AddScoped<SignUpUseCase>();
    }
}