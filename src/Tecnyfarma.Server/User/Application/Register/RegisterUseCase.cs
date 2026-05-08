using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.Register;

public class RegisterUseCase(UserRepository repository)
{
    public virtual async Task<Either<Error, Unit>> Execute(UseCaseArgs args)
    {
        return await (
            from email in Email.Create(args.Email).ToAsync()
            from password in Password.Create(args.Password).ToAsync()
            let user = new Domain.User(email, password)
            from result in repository.SaveAsync(user).ToAsync()
            select result
        );
    }
}