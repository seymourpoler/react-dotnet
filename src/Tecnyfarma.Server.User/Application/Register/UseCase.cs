using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.Register;

public class UseCase(UserRepository repository)
{
    public virtual async Task<Either<Error, Unit>> Execute(Args args)
    {
        return await (
            from email in Email.Create(args.Email).ToAsync()
            from password in Password.Create(args.Password).ToAsync()
            from _ in EnsureThatTheNewUserIsNotAlreadyRegistered(email).ToAsync()
            let user = new Domain.User(email, password)
            from result in repository.SaveAsync(user).ToAsync()
            select result
        );
    }

    private async Task<Either<Error, Unit>> EnsureThatTheNewUserIsNotAlreadyRegistered(Email email)
    {
        var found = await repository.FindAsync(email);
        return found.Match<Either<Error, Unit>>(
            _ => new Error("User already registered"),
            _ => Unit.Default
        );
    }
}