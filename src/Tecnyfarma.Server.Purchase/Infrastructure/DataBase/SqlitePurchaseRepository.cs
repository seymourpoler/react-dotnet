using LanguageExt;
using Tecnyfarma.Server.Purchase.Application.Purchase;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Infrastructure.DataBase;

public class SqlitePurchaseRepository(DbContext dbContext) : PurchaseRepository
{
    public async Task<Either<Error, Unit>> SavePurchaseAsync(Domain.Purchase purchase)
    {
        try
        {
            await dbContext.AddAsync(
                new Models.Purchase
                {
                    Id = purchase.Id,
                    Email = purchase.Email,
                    ProductId = purchase.IdProduct,
                    Price = purchase.Price,
                    CreatedAtUtc = purchase.CreatedAtUtc
                }
            );
            await dbContext.SaveChangesAsync();
            return Unit.Default;
        }
        catch (Exception exception)
        {
            return new Error(exception.Message);
        }
    }
}