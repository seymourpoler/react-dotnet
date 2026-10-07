using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Application.Purchase;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Test.Application.Purchase;

public class CreatePurchaseUseCaseShould
{
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
        userRepository.FindUserAsync("a@email.com").Returns(new Error("error"));
        
        var result = await useCase.ExecuteAsync(Guid.NewGuid(), "a@email.com");

        result.Match(
            Right: _ => Assert.True(true, "should not be here"),
            Left: error => error.Message.ShouldBe("error")
        );
    }
}