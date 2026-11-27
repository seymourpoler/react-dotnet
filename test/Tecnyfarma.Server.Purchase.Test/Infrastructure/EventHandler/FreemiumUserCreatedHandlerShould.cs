using NSubstitute;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Infrastructure.EventHandler;
using Tecnyfarma.Server.User.Message;
using Xunit;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure.EventHandler;

public class FreemiumUserCreatedHandlerShould
{
    private readonly UseCase useCase;
    private readonly FreemiumUserCreatedHandler handler;

    public FreemiumUserCreatedHandlerShould()
    {
        useCase = Substitute.For<UseCase>(Substitute.For<UserRepository>());
        handler = new FreemiumUserCreatedHandler(useCase);
    }
    
    [Fact]
    public async Task CreateFreemiumUser()
    {
        var anEvent = new FreemiumUserCreated{
            Email = "test@example.com"
        };
        
        await handler.Handle(anEvent);
        
        await useCase.Received(1).ExecuteAsync(anEvent.Email, Domain.UserType.Freemium);
    }
}