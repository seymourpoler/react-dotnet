using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application.Purchase;
using Tecnyfarma.Server.User.Domain;
using Xunit;

namespace Tecnyfarma.Server.User.Test.Application.Purchase;

public class FindPurchasesUseCaseShould
{
    const string email = "an@email.com";
    
    private readonly PurchaseRepository repository;
    private readonly FindPurchasesUseCase useCase;

    public FindPurchasesUseCaseShould()
    {
        repository = Substitute.For<PurchaseRepository>();
        useCase = new FindPurchasesUseCase(repository);
    }

    [Fact]
    public async Task ReturnPurchases()
    {
        var purchases = Array.Empty<Domain.Purchase>();
        repository.FindByAsync(Arg.Any<Email>()).Returns(purchases);
        
        var result = await useCase.ExecuteAsync(email);

        result.Match(
            Left: _ => Assert.Fail("Itshouldn't be here"),
            Right: x => x.ShouldBeOfType<Unit>()
        );
    }

    [Fact]
    public async Task ReturnErrorwhenThereIsSomeThingGoesWrong()
    {
        repository.FindByAsync(Arg.Any<Email>()).Returns(new Error("There is something goes wrong"));

        var result = await useCase.ExecuteAsync(email);

        result.Match(
            Right: _ => Assert.Fail("Itshouldn't be here"),
            Left: error => error.Message.ShouldBe("There is something goes wrong")
        );
    }
}