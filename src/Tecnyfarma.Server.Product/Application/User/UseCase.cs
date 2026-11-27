using LanguageExt;
using Tecnyfarma.Server.Product.Domain;

namespace Tecnyfarma.Server.Product.Application.User;

public class UseCase(Repository repository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(string email, UserType type)
    {
        var user = new Tecnyfarma.Server.Product.Domain.User(email, type);
        return await (
            from result in repository.SaveUserAsync(user).ToAsync()
            select result
        );
    }
}