using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application;

public interface UserRepository
{
    Task<Either<Error, Unit>> SaveAsync(Domain.User user);
    Task<Option<Domain.User>> FindAsync(Email email);
}