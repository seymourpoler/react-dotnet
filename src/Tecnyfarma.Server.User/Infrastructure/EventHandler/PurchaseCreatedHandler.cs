using Tecnyfarma.Server.Purchase.Message;
using Tecnyfarma.Server.User.Application.Purchase;

namespace Tecnyfarma.Server.User.Infrastructure.EventHandler;

public class PurchaseCreatedHandler(CreatePurchaseUseCase useCase)
{
    public async Task Handle(PurchaseCreated purchaseCreated)
    {
        var args = new CreatePurchaseArgs(
            purchaseCreated.Email,
            purchaseCreated.Name,
            purchaseCreated.Price,
            purchaseCreated.CreatedAtUtc
        );
        
        await  useCase.ExecuteAsync(args);
    }
}