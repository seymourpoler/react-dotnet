using LanguageExt;
using Tecnyfarma.Server.Product.Application.User;

namespace Tecnyfarma.Server.Product.Application.Poduct;

public class FindProductsUseCase(UserRepository userRepository, ProductRepository productRepository)
{
    public virtual async Task<Either<Domain.Error, List<Domain.Product>>> ExecuteAsync(Args args)
    {
        return await (
            from user in userRepository.FindUserAsync(args.Email).ToAsync()
            from products in productRepository.FindProductsAsync().ToAsync()
            select products.Select(p => new Domain.Product(p.Id, p.Name, p.Description, user.CalculatePrice(p.Price))).ToList()
        );
    }
}