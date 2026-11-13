using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.Purchase;

public class FindPurchasesUseCase(PurchaseRepository repository)
{
    public virtual async Task<Either<Error, Domain.Purchase[]>> ExecuteAsync(string email)
    {
        throw new NotImplementedException();
    }
}