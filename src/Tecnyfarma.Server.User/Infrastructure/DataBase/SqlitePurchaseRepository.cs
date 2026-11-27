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

    public async Task<Either<Error, Domain.Purchase[]>> FindByAsync(Email email)
    {
        return dbContext.Purchases
            .Where(x => x.Email == email.Value)
            .Select(x => new Domain.Purchase(x.Email, x.Name, x.Price, x.CreatedAtUtc))
            .ToArray();
    }
}