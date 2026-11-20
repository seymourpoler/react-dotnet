using LanguageExt;
using Tecnyfarma.Server.Product.Domain;

namespace Tecnyfarma.Server.Product.Application.Product.Create;

public class CreateProductUseCase(ProductRepository repository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(Args args)
    {
        return await (
            from product in Domain.Product.Create(args.Name, args.Description, args.Price).ToAsync()
            from result in repository.SaveAsync(product).ToAsync()
            select result
        );
    }
}