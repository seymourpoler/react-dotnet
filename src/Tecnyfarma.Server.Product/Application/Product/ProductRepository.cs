using LanguageExt;
using Tecnyfarma.Server.Product.Domain;

namespace Tecnyfarma.Server.Product.Application.Product;

public interface ProductRepository
{
    Task<Either<Error, List<Domain.Product>>> FindProductsAsync();
}