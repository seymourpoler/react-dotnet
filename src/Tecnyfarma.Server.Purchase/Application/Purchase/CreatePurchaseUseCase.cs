using LanguageExt;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Application.Purchase;

public class CreatePurchaseUseCase(UserRepository userRepository, ProductRepository productRepository, PurchaseRepository purchaseRepository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(Guid idProduct, string email)
    {
        return await EitherAsync<Error, Unit>.From(() => userRepository.FindUserAsync(email))
            .Bind(user => EitherAsync<Error, Unit>.From(() => productRepository.FindProductAsync(idProduct))
                .Bind(product => EitherAsync<Error, Unit>.From(() => Domain.Purchase.Create(user, product))
                    .Bind(purchase => EitherAsync<Error, Unit>.From(() => purchaseRepository.SavePurchaseAsync(purchase)))));
    }
}