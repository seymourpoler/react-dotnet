using LanguageExt;
using LanguageExt.Common;

namespace Tecnyfarma.Server.Purchase.Application.Purchase;

public interface PurchaseRepository
{
    Task<Either<Error, Unit>> SavePurchaseAsync(Domain.Purchase purchase);
}