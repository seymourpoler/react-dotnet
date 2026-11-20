using LanguageExt;
using Tecnyfarma.Server.Product.Domain;
using Tecnyfarma.Server.Product.Message;
using Wolverine;

namespace Tecnyfarma.Server.Product.Application.Product.Create;

public class CreateProductUseCase(ProductRepository repository, IMessageBus bus )
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(Args args)
    {
        return await (
            from product in Domain.Product.Create(args.Name, args.Description, args.Price).ToAsync()
            from result in repository.SaveAsync(product).ToAsync()
            from __ in PublishProductCreated(product).ToAsync()
            select result
        );
    }

    private async Task<Either<Error, Unit>> PublishProductCreated(Domain.Product product)
    {
        await bus.PublishAsync(new ProductCreated
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        });
        return Either<Error, Unit>.Right(Unit.Default);
    }
}