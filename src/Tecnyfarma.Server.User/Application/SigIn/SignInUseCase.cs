using System;
using System.Threading.Tasks;
using LanguageExt;

namespace Tecnyfarma.Server.User.Application.SigIn;

public class SignInUseCase(UserRepository repository)
{
    public async Task<Either<Error, Unit>> Execute(UseCaseArgs args)
    {
        throw new NotImplementedException();
    }
}