using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.Purchase.Find;

public class UseCase(Repository repository)
{
    public virtual async Task<Either<Error, Domain.Purchase[]>> ExecuteAsync(string email)
    {
        return await (
            from theEmail in Email.Create(email).ToAsync()
            from result in repository.FindByAsync(theEmail).ToAsync()
            select result
        );
    }
}