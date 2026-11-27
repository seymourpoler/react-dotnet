using LanguageExt;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Application.Product;

public class UseCase(ProductRepository productRepository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(Args args)
    {
        return await (
            from product in Domain.Product.Create(args.IdProduct, args.Name, args.Price).ToAsync()
            from result in productRepository.SaveProductAsync(product).ToAsync()
            select result
        );
    }
}