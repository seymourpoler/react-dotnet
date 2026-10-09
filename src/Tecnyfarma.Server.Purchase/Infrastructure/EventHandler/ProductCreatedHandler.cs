using Tecnyfarma.Server.Product.Message;
using Tecnyfarma.Server.Purchase.Application.Product;

namespace Tecnyfarma.Server.Purchase.Infrastructure.EventHandler;

public class ProductCreatedHandler(CreateProductUseCase useCase)
{
    public async Task Handle(ProductCreated userCreated)
    {
        var args = new CreateProductArgs(userCreated.Id, userCreated.Name, userCreated.Price);
        await useCase.ExecuteAsync(args);
    }
}