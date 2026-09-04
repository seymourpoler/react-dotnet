using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Tecnyfarma.Server.User.Infrastructure;
using Tecnyfarma.Server.User.Infrastructure.DataBase;

namespace Tecnyfarma.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/api/v0/users/login";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
        builder.Services.AddControllers();
        builder.Services.AddUserDependencies(builder.Configuration);

        var app = builder.Build();
        app.UseDefaultFiles();
        app.MapStaticAssets();

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapFallbackToFile("/index.html");

        using (var scope = app.Services.CreateScope())
        {
            var usersDb = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            usersDb.Database.Migrate();
        }
        
        app.Run();
    }
}