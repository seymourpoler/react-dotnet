using LanguageExt;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Application.Product;

public class CreateProductUseCase(ProductRepository repository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(CreateProductArgs args)
    {
        return await (
            from product in Domain.Product.Create(args.IdProduct, args.Name, args.Price).ToAsync()
            from result in repository.SaveProductAsync(product).ToAsync()
            select result
        );
    }
}