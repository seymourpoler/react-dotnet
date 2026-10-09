using LanguageExt;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Application.Purchase;

public class CreatePurchaseUseCase(UserRepository userRepository, ProductRepository productRepository, PurchaseRepository purchaseRepository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(Guid idProduct, string email)
    {
        return await (
            from user in userRepository.FindUserAsync(email).ToAsync()
            from product in productRepository.FindProductAsync(idProduct).ToAsync()
            from purchase in Domain.Purchase.Create(user, product).ToAsync()
            from result in purchaseRepository.SavePurchaseAsync(purchase).ToAsync()
            select result
        );
    }
}