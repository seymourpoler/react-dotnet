using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application;

public class RegisterUserUseCase(UserRepository repository)
{
    public virtual async Task<Either<Error, Unit>> Execute(RegisterUserArgs args)
    {
        var pp = await (
            from email in Email.Create(args.Email).ToAsync()
            from password in Password.Create(args.Password).ToAsync()
            let user = new Domain.User(email, password)
            from a in repository.SaveAsync(user).ToAsync()
            select a
        );
        return pp;
    }
}