using LanguageExt;

namespace Tecnyfarma.Server.Purchase.Application.User;

public interface UserRepository
{
    Task<Either<Domain.Error, Tecnyfarma.Server.Purchase.Domain.User>> FindUserAsync(string email);
    Task<Either<Domain.Error, Unit>> SaveUserAsync(Tecnyfarma.Server.Purchase.Domain.User user);
}