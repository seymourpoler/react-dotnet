using NSubstitute;
using Tecnyfarma.Server.Purchase.Message;
using Tecnyfarma.Server.User.Application.Purchase;
using Tecnyfarma.Server.User.Application.Purchase.Create;
using Tecnyfarma.Server.User.Infrastructure.EventHandler;
using Xunit;

namespace Tecnyfarma.Server.User.Test.Infrastructure.EventHandler;

public class PurchaseCreatedHandlerShould
{
    private readonly UseCase useCase;
    private readonly PurchaseCreatedHandler handler;

    public PurchaseCreatedHandlerShould()
    {
        useCase = Substitute.For<UseCase>(Substitute.For<Repository>());
        handler = new PurchaseCreatedHandler(useCase);
    }

    [Fact]
    public async Task CreatePurchase()
    {
        var anEvent = new PurchaseCreated
        {
            Email = "an@email.com",
            Name = "any-name",
            Price = 10,
            CreatedAtUtc = new DateTime(2026,01,01)
        };

        await handler.Handle(anEvent);

        useCase.Received().ExecuteAsync(Arg.Is<Args>(x =>
                x.Email == "an@email.com" &&
                x.Name == "any-name" &&
                x.Price == 10 &&
                x.CreatedAtUtc == new DateTime(2026, 01, 01)
            )
        );
    }
}