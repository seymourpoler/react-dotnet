using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.Purchase.Create;

public class UseCase(Repository repository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(Args args)
    {
        var purchase = new Domain.Purchase(args.Email, args.Name, args.Price, args.CreatedAtUtc);
        return await repository.SaveAsync(purchase);
    }
}