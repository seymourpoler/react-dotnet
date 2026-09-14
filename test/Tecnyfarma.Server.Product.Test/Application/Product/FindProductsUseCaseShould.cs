using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.Product.Application.Product;
using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.Product.Domain;
using Xunit;

namespace Tecnyfarma.Server.Product.Test.Application.Product;

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

    [Fact]
    public async Task ReturnProductsWithTheUserPriceForAFreemiumUser()
    {
        var user = new Tecnyfarma.Server.Product.Domain.User("user@example.com", UserType.Freemium);
        var products = new List<Tecnyfarma.Server.Product.Domain.Product>
        {
            new(Guid.NewGuid(), "Product 1", "Description 1", 100f)
        };
        userRepository.FindUserAsync("user@example.com").Returns(user);
        productRepository.FindProductsAsync().Returns(products);

        var result = await findProductsUseCase.ExecuteAsync(new Args("user@example.com"));

        result.Match(x =>
            {
                var product = x.ShouldHaveSingleItem();
                product.Price.ShouldBe(100f);
            },
            _ => Assert.Fail("Expected products but got an error")
        );
        await userRepository.Received(1).FindUserAsync("user@example.com");
        await productRepository.Received(1).FindProductsAsync();
    }

    [Fact]
    public async Task ApplyThePremiumDiscountToTheProductPrices()
    {
        var user = new Tecnyfarma.Server.Product.Domain.User("user@example.com", UserType.Premium);
        var products = new List<Tecnyfarma.Server.Product.Domain.Product>
        {
            new(Guid.NewGuid(), "Product 1", "Description 1", 100f)
        };
        userRepository.FindUserAsync("user@example.com").Returns(user);
        productRepository.FindProductsAsync().Returns(products);

        var result = await findProductsUseCase.ExecuteAsync(new Args("user@example.com"));

        result.Match(
            foundProducts =>
            {
                var product = foundProducts.ShouldHaveSingleItem();
                product.Price.ShouldBe(90f, 0.0001);
            },
            _ => Assert.Fail("Expected products but got an error")
        );
    }

    [Fact]
    public async Task ReturnTheErrorWhenTheUserDoesNotExist()
    {
        userRepository.FindUserAsync(Arg.Any<string>()).Returns(new Error("User not found"));

        var result = await findProductsUseCase.ExecuteAsync(new Args("user@example.com"));

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("User not found")
        );
        await productRepository.DidNotReceive().FindProductsAsync();
    }

    [Fact]
    public async Task ReturnTheErrorWhenTheProductsCannotBeLoaded()
    {
        var user = new Tecnyfarma.Server.Product.Domain.User("user@example.com", UserType.Freemium);
        userRepository.FindUserAsync("user@example.com").Returns(user);
        productRepository.FindProductsAsync().Returns(new Error("Database error"));

        var result = await findProductsUseCase.ExecuteAsync(new Args("user@example.com"));

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("Database error")
        );
        await userRepository.Received(1).FindUserAsync("user@example.com");
    }
}