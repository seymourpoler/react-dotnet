using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.Login;

public class LoginUseCase(UserRepository repository)
{
    public virtual async Task<Either<Error, Unit>> Execute(UseCaseArgs args)
    {
        var result = await (
            from email in Email.Create(args.Email).ToAsync()
            from password in Password.Create(args.Password).ToAsync()
            from user in repository.FindAsync(email).ToAsync()
            select user.IsEqualTo(new Domain.User(email, password))
        );
        return result.Match<Either<Error, Unit>>(
            Right: isOk => isOk ? Unit.Default : new Error("Invalid credentials"),
            Left: error => error
        );
    }
}