using LanguageExt;
using Tecnyfarma.Server.Product.Application.User;

namespace Tecnyfarma.Server.Product.Application.Product.Find;

public class UseCase(
    Tecnyfarma.Server.Product.Application.User.Repository userRepository,
    Tecnyfarma.Server.Product.Application.Product.Repository repository)
{
    public virtual async Task<Either<Domain.Error, List<Domain.Product>>> ExecuteAsync(Args args)
    {
        return await (
            from user in userRepository.FindUserAsync(args.Email).ToAsync()
            from products in repository.FindProductsAsync().ToAsync()
            select products.Select(p => new Domain.Product(p.Id, p.Name, p.Description, user.CalculatePrice(p.Price))).ToList()
        );
    }
}