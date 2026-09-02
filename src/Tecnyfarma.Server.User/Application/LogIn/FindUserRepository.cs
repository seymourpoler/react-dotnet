using LanguageExt;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Application.LogIn;

public interface FindUserRepository
{
    Task<Either<Error, Domain.User>> FindAsync(Email email);
}