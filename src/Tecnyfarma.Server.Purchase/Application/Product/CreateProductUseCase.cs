using LanguageExt;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Application.Product;

public class CreateProductUseCase(ProductRepository repository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(CreateProductArgs args)
    {
        throw new NotImplementedException();
    }
}