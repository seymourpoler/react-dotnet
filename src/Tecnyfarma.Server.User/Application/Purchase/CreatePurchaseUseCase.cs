using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.Purchase;

public class CreatePurchaseUseCase(PurchaseRepository repository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(CreatePurchaseArgs args)
    {
        var purchase = new Domain.Purchase(args.Email, args.Name, args.Price, args.CreatedAtUtc);
        return await repository.SaveAsync(purchase);
    }
}