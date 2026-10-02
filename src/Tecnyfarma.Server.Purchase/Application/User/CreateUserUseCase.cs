using LanguageExt;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Application.User;

public class CreateUserUseCase(UserRepository repository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(string email, UserType userType)
    {
        var user = new Domain.User(email, userType);
        return await (
            from result in repository.SaveUserAsync(user).ToAsync()
            select result
        );
    }
}