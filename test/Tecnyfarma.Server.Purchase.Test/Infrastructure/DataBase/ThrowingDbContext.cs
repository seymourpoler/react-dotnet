using Microsoft.EntityFrameworkCore;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure.DataBase;

internal sealed class ThrowingDbContext(DbContextOptions<Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext> options) : Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext(options)
{
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Save failed");
    }
}
