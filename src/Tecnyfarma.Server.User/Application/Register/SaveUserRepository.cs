using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.Register;

public interface SaveUserRepository
{
    Task<Either<Error, Unit>> SaveAsync(Domain.User user);
}