using LanguageExt;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Domain;
using Tecnyfarma.Server.Purchase.Message;
using Wolverine;

namespace Tecnyfarma.Server.Purchase.Application.Purchase;

public class CreatePurchaseUseCase(UserRepository userRepository, ProductRepository productRepository, PurchaseRepository purchaseRepository, IMessageBus bus)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(Guid idProduct, string email)
    {
        return await (
            from user in userRepository.FindUserAsync(email).ToAsync()
            from product in productRepository.FindProductAsync(idProduct).ToAsync()
            from purchase in Domain.Purchase.Create(user, product).ToAsync()
            from result in purchaseRepository.SavePurchaseAsync(purchase).ToAsync()
            from __ in PublishPurchaseCreated(product, purchase).ToAsync()
            select result
        );
    }

    private async Task<Either<Error, Unit>> PublishPurchaseCreated(Domain.Product product, Domain.Purchase purchase)
    {
        var theEvent = new PurchaseCreated
        {
            Email = purchase.Email,
            Name = product.Name,
            Price = purchase.Price,
            CreatedAtUtc = purchase.CreatedAtUtc
        };
        
        await bus.SendAsync(theEvent);
        return Unit.Default;
    }
}