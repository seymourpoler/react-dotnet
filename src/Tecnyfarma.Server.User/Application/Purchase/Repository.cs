using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.Purchase;

public interface Repository
{
    Task<Either<Error, Unit>> SaveAsync(Domain.Purchase purchase);
    Task<Either<Error, Domain.Purchase[]>> FindByAsync(Email email);
}