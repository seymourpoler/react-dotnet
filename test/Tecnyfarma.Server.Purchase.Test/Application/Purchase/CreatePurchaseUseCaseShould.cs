using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Application.Purchase;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Test.Application.Purchase;

public class CreatePurchaseUseCaseShould
{
    private const string Email = "a@email.com";
    private static readonly Guid ProductId = Guid.NewGuid();

    private readonly UserRepository userRepository;
    private readonly ProductRepository productRepository;
    private readonly PurchaseRepository purchaseRepository;
    private readonly CreatePurchaseUseCase useCase;

    public CreatePurchaseUseCaseShould()
    {
        userRepository = Substitute.For<UserRepository>();
        productRepository = Substitute.For<ProductRepository>();
        purchaseRepository = Substitute.For<PurchaseRepository>();
        useCase = new CreatePurchaseUseCase(userRepository, productRepository, purchaseRepository);
    }

    [Fact]
    public async Task ReturnErrorWhenThereIsAnErrorWithUser()
    {
        userRepository.FindUserAsync(Email).Returns(new Error("error"));

        var result = await useCase.ExecuteAsync(ProductId, Email);

        result.Match(
            Right: _ => Assert.Fail("should not be here"),
            Left: error => error.Message.ShouldBe("error")
        );
        await productRepository.DidNotReceive().FindProductAsync(Arg.Any<Guid>());
        await purchaseRepository.DidNotReceive().SavePurchaseAsync(Arg.Any<Domain.Purchase>());
    }
    
    [Fact]
    public async Task ReturnErrorWhenThereIsAnErrorWithProduct()
    {
        GivenAUser();
        productRepository.FindProductAsync(ProductId).Returns(new Error("product error"));

        var result = await useCase.ExecuteAsync(ProductId, Email);

        result.Match(
            Right: _ => Assert.Fail("Expected an error but got a success result"),
            Left: error => error.Message.ShouldBe("product error")
        );
        await purchaseRepository.DidNotReceive().SavePurchaseAsync(Arg.Any<Domain.Purchase>());
    }

    [Fact]
    public async Task ReturnErrorWhenThePurchaseCannotBeSaved()
    {
        GivenAUser();
        GivenAProduct();
        purchaseRepository.SavePurchaseAsync(Arg.Any<Domain.Purchase>()).Returns(new Error("database error"));

        var result = await useCase.ExecuteAsync(ProductId, Email);

        result.Match(
            Right: _ => Assert.Fail("Expected an error but got a success result"),
            Left: error => error.Message.ShouldBe("database error")
        );
        await userRepository.Received(1).FindUserAsync(Email);
        await productRepository.Received(1).FindProductAsync(ProductId);
    }

    [Theory]
    [InlineData(UserType.Freemium, 100f, 100f)]
    [InlineData(UserType.Premium, 100f, 90f)]
    public async Task SaveThePurchaseWithThePriceCalculatedFromTheUserType(UserType type, float productPrice, float expectedPrice)
    {
        GivenAUser(type);
        GivenAProduct(productPrice);

        await useCase.ExecuteAsync(ProductId, Email);

        await purchaseRepository.Received(1).SavePurchaseAsync(Arg.Is<Domain.Purchase>(purchase =>
            Math.Abs(purchase.Price - expectedPrice) < 0.01f));
    }
    
    [Fact]
    public async Task CreatePurchase()
    {
        GivenAUser();
        GivenAProduct();
        purchaseRepository.SavePurchaseAsync(Arg.Any<Domain.Purchase>()).Returns(Unit.Default);

        var result = await useCase.ExecuteAsync(ProductId, Email);

        result.Match(
            Left: _ => Assert.Fail("Expected an error"),
            Right: x => x.ShouldBeOfType<Unit>()
        );
        await purchaseRepository.Received(1).SavePurchaseAsync(Arg.Is<Domain.Purchase>(purchase =>
            purchase.Email == Email &&
            purchase.IdProduct == ProductId)
        );
    }

    private void GivenAUser(UserType type = UserType.Freemium)
    {
        userRepository.FindUserAsync(Email).Returns(Domain.User.Create(Email, type));
    }

    private void GivenAProduct(float price = 100f)
    {
        productRepository.FindProductAsync(ProductId).Returns(Domain.Product.Create(ProductId, "Product", price));
    }
}
