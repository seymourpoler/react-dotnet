using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.Product.Application.Product;
using Tecnyfarma.Server.Product.Application.Product.Create;
using Tecnyfarma.Server.Product.Domain;
using Tecnyfarma.Server.Product.Message;
using Wolverine;
using Xunit;

namespace Tecnyfarma.Server.Product.Test.Application.Product.Create;

public class UseCaseShould
{
    private const string Name = "Product";
    private const string Description = "Description";
    private const float Price = 100f;

    private readonly ProductRepository repository;
    private readonly IMessageBus bus;
    private readonly UseCase useCase;

    public UseCaseShould()
    {
        repository = Substitute.For<ProductRepository>();
        bus = Substitute.For<IMessageBus>();
        useCase = new UseCase(repository, bus);
    }

    [Fact]
    public async Task SaveTheProduct()
    {
        repository.SaveAsync(Arg.Is<Tecnyfarma.Server.Product.Domain.Product>(product =>
            product.Name == Name &&
            product.Description == Description &&
            Math.Abs(product.Price - Price) < 0.01f &&
            product.Id != Guid.Empty)).Returns(Unit.Default);
        var args = new Args(Name, Description, Price);

        var result = await useCase.ExecuteAsync(args);

        result.Match(
            Left: _ => Assert.Fail("Expected success but got an error"),
            Right: x => x.ShouldBeOfType<Unit>()
        );
        await bus.Received().PublishAsync(Arg.Is<ProductCreated>(x => x.Name == Name && x.Price == Price));
    }

    [Fact]
    public async Task ReturnErrorWhenTheProductCannotBeSaved()
    {
        repository.SaveAsync(Arg.Any<Tecnyfarma.Server.Product.Domain.Product>()).Returns(new Error("database error"));
        var args = new Args(Name, Description, Price);

        var result = await useCase.ExecuteAsync(args);

        result.Match(
            Right: _ => Assert.Fail("Expected an error but got a success result"),
            Left: error => error.Message.ShouldBe("database error")
        );
        await repository.Received(1).SaveAsync(Arg.Any<Tecnyfarma.Server.Product.Domain.Product>());
        await bus.DidNotReceive().PublishAsync(Arg.Any<ProductCreated>());
    }

    [Theory]
    [InlineData(null, Description, Price, "Product name is required")]
    [InlineData("", Description, Price, "Product name is required")]
    [InlineData("   ", Description, Price, "Product name is required")]
    [InlineData(Name, null, Price, "Product description is required")]
    [InlineData(Name, "", Price, "Product description is required")]
    [InlineData(Name, "   ", Price, "Product description is required")]
    [InlineData(Name, Description, 0f, "Product price must be greater than zero")]
    [InlineData(Name, Description, -1f, "Product price must be greater than zero")]
    public async Task ReturnErrorWhenTheProductIsInvalid(string name, string description, float price, string expectedError)
    {
        var args = new Args(name, description, price);

        var result = await useCase.ExecuteAsync(args);

        result.Match(
            Right: _ => Assert.Fail("Expected an error but got a success result"),
            Left: error => error.Message.ShouldBe(expectedError)
        );
        await repository.DidNotReceive().SaveAsync(Arg.Any<Tecnyfarma.Server.Product.Domain.Product>());
        await bus.DidNotReceive().PublishAsync(Arg.Any<ProductCreated>());
    }
}