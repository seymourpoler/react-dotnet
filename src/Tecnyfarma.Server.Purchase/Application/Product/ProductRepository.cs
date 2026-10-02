using LanguageExt;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Application.Product;

public interface ProductRepository
{
    Task<Either<Error, Domain.Product>> FindProductAsync(Guid idProduct);
    Task<Either<Error, Unit>> SaveProductAsync(Domain.Product product);
}