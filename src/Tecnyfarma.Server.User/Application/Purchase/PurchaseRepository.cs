using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.Purchase;

public interface PurchaseRepository
{
    Task<Either<Error, Unit>> SaveAsync(Domain.Purchase purchase);
}