using Tecnyfarma.Server.User.Application;

namespace Tecnyfarma.Server.User.Infrastructure;

public class SqlUserRepository : UserRepository
{
    public Task SaveAsync(Domain.User user)
    {
        throw new NotImplementedException();
    }
}