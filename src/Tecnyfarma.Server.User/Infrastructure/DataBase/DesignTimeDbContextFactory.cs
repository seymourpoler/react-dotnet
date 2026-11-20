using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tecnyfarma.Server.User.Infrastructure.DataBase;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DbContext>
{
    public DbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseSqlite("Data Source=../Tecnyfarma.Server.User/Infrastructure/DataBase/users.sqlite")
            .Options;

        return new DbContext(options);
    }
}
