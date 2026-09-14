using NSubstitute;
using Tecnyfarma.Server.Product.Application.Poduct;
using Tecnyfarma.Server.Product.Application.User;

namespace Tecnyfarma.Server.Product.Test.Application;

public class FindProductsUseCaseShould
{
    private readonly UserRepository userRepository;
    private readonly ProductRepository productRepository;
    private readonly FindProductsUseCase findProductsUseCase;

    public FindProductsUseCaseShould()
    {
        userRepository = Substitute.For<UserRepository>();
        productRepository = Substitute.For<ProductRepository>();
        findProductsUseCase = new FindProductsUseCase(userRepository, productRepository);
    }
}