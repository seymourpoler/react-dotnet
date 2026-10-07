using LanguageExt;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Application.Purchase;

public class CreatePurchaseUseCase(UserRepository userRepository, ProductRepository productRepository, PurchaseRepository purchaseRepository)
{
    public virtual async Task<Either<Error, Unit>> ExecuteAsync(Guid idProduct, string email)
    {
        var userResult = await userRepository.FindUserAsync(email);
        var productResult = await productRepository.FindProductAsync(idProduct);

        return userResult.Match(
            Left: error => Either<Error, Unit>.Left(error),
            Right: user => productResult
                .Match(Left: error => Either<Error, Unit>.Left(error),
                    Right: product => Domain.Purchase.Create(user, product)
                        .Match(Left: error => Either<Error, Unit>.Left(error),
                            Right: purchase => purchaseRepository.SavePurchaseAsync(purchase)
                        )
                )
        );
    }
}