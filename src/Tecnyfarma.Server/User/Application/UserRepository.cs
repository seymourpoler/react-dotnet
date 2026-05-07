using LanguageExt;

namespace Tecnyfarma.Server.User.Application;

public interface UserRepository
{
    Task<Either<Error, Unit>> SaveAsync(Domain.User user);
}