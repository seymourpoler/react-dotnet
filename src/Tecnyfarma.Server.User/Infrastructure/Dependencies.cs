using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Tecnyfarma.Server.User.Application.LogIn;
using Tecnyfarma.Server.User.Application.Register;
using Tecnyfarma.Server.User.Infrastructure.DataBase;

namespace Tecnyfarma.Server.User.Infrastructure;

public static class Dependencies
{
    public static void AddUserDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UsersDatabase") ?? "Data Source=users.db";
        services.AddDbContext<UsersDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<SaveUserRepository, SqliteUserRepository>();
        services.AddScoped<FindUserRepository, SqliteUserRepository>();
        services.AddScoped<Application.LogIn.UseCase>();
    }
}