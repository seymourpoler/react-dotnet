using Tecnyfarma.Server.Purchase.Message;
using Tecnyfarma.Server.User.Application.Purchase;
using Tecnyfarma.Server.User.Application.Purchase.Create;

namespace Tecnyfarma.Server.User.Infrastructure.EventHandler;

public class PurchaseCreatedHandler(UseCase useCase)
{
    public async Task Handle(PurchaseCreated purchaseCreated)
    {
        var args = new Args(
            purchaseCreated.Email,
            purchaseCreated.Name,
            purchaseCreated.Price,
            purchaseCreated.CreatedAtUtc
        );
        
        await  useCase.ExecuteAsync(args);
    }
}