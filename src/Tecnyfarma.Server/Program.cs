using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Tecnyfarma.Server.Product.Infrastructure;
using Tecnyfarma.Server.User.Infrastructure;
using Wolverine;

namespace Tecnyfarma.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseWolverine(opts =>
        {
            opts.MultipleHandlerBehavior = MultipleHandlerBehavior.Separated;
            opts.Discovery.IncludeAssembly(typeof(Tecnyfarma.Server.Product.Infrastructure.Dependencies).Assembly);
        });
        
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
        builder.Services.AddProductDependencies(builder.Configuration);

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
            var usersDb = scope.ServiceProvider.GetRequiredService<Tecnyfarma.Server.User.Infrastructure.DataBase.DbContext>();
            usersDb.Database.Migrate();
            var productsDb = scope.ServiceProvider.GetRequiredService<Tecnyfarma.Server.Product.Infrastructure.DataBase.DbContext>();
            productsDb.Database.Migrate();
        }
        
        app.Run();
    }
}