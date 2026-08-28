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
        builder.Services.AddControllers();
        builder.Services.AddUserDependencies(builder.Configuration);

        var app = builder.Build();
        app.UseDefaultFiles();
        app.MapStaticAssets();

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();
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