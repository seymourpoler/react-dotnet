using LanguageExt;
using Tecnyfarma.Server.Product.Domain;

namespace Tecnyfarma.Server.Product.Application.Product.Create;

public class CreateProductUseCase(ProductRepository repository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(Args args)
    {
        throw new NotImplementedException();
    }
}