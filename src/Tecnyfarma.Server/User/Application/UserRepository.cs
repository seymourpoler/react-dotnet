namespace Tecnyfarma.Server.User.Application;

public interface UserRepository
{
    Task SaveAsync(Domain.User user);
}