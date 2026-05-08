using LanguageExt;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Infrastructure;

public class SqlUserRepository : UserRepository
{
    public Task<Either<Error, Unit>> SaveAsync(Domain.User user)
    {
        throw new NotImplementedException();
    }

    public Task<Option<Domain.User>> FindAsync(Email email)
    {
        throw new NotImplementedException();
    }
}