using LanguageExt;
using Tecnyfarma.Server.Product.Domain;

namespace Tecnyfarma.Server.Product.Application.User;

public interface Repository
{
    Task<Either<Error, Tecnyfarma.Server.Product.Domain.User>> FindUserAsync(string email);
    Task<Either<Error, Unit>> SaveUserAsync(Tecnyfarma.Server.Product.Domain.User user);
    
}