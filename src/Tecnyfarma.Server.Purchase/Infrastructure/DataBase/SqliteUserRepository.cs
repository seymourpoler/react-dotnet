using LanguageExt;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Infrastructure.DataBase;

public class SqliteUserRepository : UserRepository
{
    public Task<Either<Error, Domain.User>> FindUserAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Either<Error, Unit>> SaveUserAsync(Domain.User user)
    {
        throw new NotImplementedException();
    }
}