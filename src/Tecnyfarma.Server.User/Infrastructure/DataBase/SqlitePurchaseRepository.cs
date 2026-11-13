using LanguageExt;
using Tecnyfarma.Server.User.Application.Purchase;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Infrastructure.DataBase;

public class SqlitePurchaseRepository(DbContext dbContext) : PurchaseRepository
{
    public async Task<Either<Error, Unit>> SaveAsync(Domain.Purchase purchase)
    {
        var databasePurchase = new Models.Purchase
        {
            Email = purchase.Email,
            Name = purchase.Name,
            Price = purchase.Price,
            CreatedAtUtc = purchase.CreatedAtUtc
        };
        await dbContext.Purchases.AddAsync(databasePurchase);
        await dbContext.SaveChangesAsync();
        return Unit.Default;    
    }
}