using Tecnyfarma.Server.Product.Message;
using Tecnyfarma.Server.Purchase.Application.Product;

namespace Tecnyfarma.Server.Purchase.Infrastructure.EventHandler;

public class ProductCreatedHandler(UseCase useCase)
{
    public async Task Handle(ProductCreated userCreated)
    {
        var args = new Args(userCreated.Id, userCreated.Name, userCreated.Price);
        await useCase.ExecuteAsync(args);
    }
}