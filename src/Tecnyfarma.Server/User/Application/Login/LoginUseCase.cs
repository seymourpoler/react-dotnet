using LanguageExt;

namespace Tecnyfarma.Server.User.Application.Login;

public class LoginUseCase(UserRepository repository)
{
    public virtual async Task<Either<Error, Unit>> Execute(UseCaseArgs args)
    {
        throw new NotImplementedException();
    }
}