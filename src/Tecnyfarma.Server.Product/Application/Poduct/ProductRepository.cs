using LanguageExt;
using Tecnyfarma.Server.Product.Domain;

namespace Tecnyfarma.Server.Product.Application.Poduct;

public interface ProductRepository
{
    Task<Either<Error, List<Domain.Product>>> FindProductsAsync();
}