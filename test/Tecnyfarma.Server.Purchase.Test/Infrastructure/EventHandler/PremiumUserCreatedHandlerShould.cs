using NSubstitute;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Infrastructure.EventHandler;
using Tecnyfarma.Server.User.Message;
using Xunit;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure.EventHandler;

public class PremiumUserCreatedHandlerShould
{
    private readonly UseCase useCase;
    private readonly PremiumUserCreatedHandler handler;

    public PremiumUserCreatedHandlerShould()
    {
        useCase = Substitute.For<UseCase>(Substitute.For<UserRepository>());
        handler = new PremiumUserCreatedHandler(useCase);
    }
    
    [Fact]
    public async Task CreatePremiumUser()
    {
        var anEvent = new PremiumUserCreated{
            Email = "test@example.com"
        };
        
        await handler.Handle(anEvent);
        
        await useCase.Received(1).ExecuteAsync(anEvent.Email, Domain.UserType.Premium);
    }
}