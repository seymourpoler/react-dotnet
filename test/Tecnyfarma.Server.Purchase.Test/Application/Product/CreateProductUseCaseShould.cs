using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Test.Application.Product;

public class CreateProductUseCaseShould
{
    private const string Name = "Product";
    private const float Price = 100f;
    private static readonly Guid ProductId = Guid.NewGuid();

    private readonly ProductRepository repository;
    private readonly CreateProductUseCase useCase;

    public CreateProductUseCaseShould()
    {
        repository = Substitute.For<ProductRepository>();
        useCase = new CreateProductUseCase(repository);
    }

    [Fact]
    public async Task SaveTheProductBuiltFromTheArguments()
    {
        repository.SaveProductAsync(Arg.Any<Domain.Product>()).Returns(Unit.Default);
        var args = new CreateProductArgs(ProductId, Name, Price);
        
        var result = await useCase.ExecuteAsync(args);

        result.Match(
            Left: _ => Assert.Fail("Expected an error but got success result"),
            Right: x => x.ShouldBeOfType<Unit>()
        );
        await repository.Received(1).SaveProductAsync(Arg.Is<Domain.Product>(product =>
            product.Id == ProductId &&
            product.Name == Name &&
            product.Price == Price));
    }

    [Fact]
    public async Task ReturnErrorWhenTheProductCannotBeSaved()
    {
        repository.SaveProductAsync(Arg.Any<Domain.Product>()).Returns(new Error("database error"));
        var args = new CreateProductArgs(ProductId, Name, Price);
        
        var result = await useCase.ExecuteAsync(args);

        result.Match(
            Right: _ => Assert.Fail("Expected an error but got a success result"),
            Left: error => error.Message.ShouldBe("database error")
        );
    }
}
