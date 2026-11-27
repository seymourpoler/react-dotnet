using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application.Purchase;
using Tecnyfarma.Server.User.Application.Purchase.Create;
using Tecnyfarma.Server.User.Domain;
using Xunit;

namespace Tecnyfarma.Server.User.Test.Application.Purchase.Create;

public class UseCaseShould
{
    private readonly  PurchaseRepository repository;
    private readonly UseCase useCase;

    public UseCaseShould()
    {
        repository = Substitute.For<PurchaseRepository>();
        useCase = new UseCase(repository);
    }

    [Fact]
    public async Task CreatePurchase()
    {
        repository.SaveAsync(Arg.Is<Domain.Purchase>(x =>
                x.Email == "ab@email.com" &&
                x.Name == "any-nae" &&
                x.Price == 10f &&
                x.CreatedAtUtc == new DateTime(2026, 01, 01)
            )
        ).Returns(Unit.Default);
        var args = new Args("ab@email.com", "any-nae", 10, new DateTime(2026,01,01));
        
        var result = await useCase.ExecuteAsync(args);

        result.Match(
            Right: x => x.ShouldBeOfType<Unit>(),
            Left: _ => Assert.Fail("Expected an error but got success result")
        );
    }

    [Fact]
    public async Task ReturnErrorWhenThePurchaseCannotBeCreated()
    {
        repository.SaveAsync(Arg.Any<Domain.Purchase>()).Returns(new Error("database error"));
        var args = new Args("ab@email.com", "any-nae", 10, new DateTime(2026,01,01));
        
        var result = await useCase.ExecuteAsync(args);
        
        result.Match(
            Right: _ => Assert.Fail("Expected an error but got a success result"),
            Left: error => error.Message.ShouldBe("database error")
        );
    }
}