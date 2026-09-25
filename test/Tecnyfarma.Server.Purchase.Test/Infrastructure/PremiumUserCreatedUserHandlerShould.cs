using NSubstitute;
using Tecnyfarma.Server.Purchase.Application;
using Tecnyfarma.Server.Purchase.Infrastructure;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure;

public class PremiumUserCreatedUserHandlerShould
{
    private readonly CreateUserUseCase createUserUseCase;
    private readonly PremiumUserCreatedUserHandler handler;

    public PremiumUserCreatedUserHandlerShould()
    {
        createUserUseCase = Substitute.For<CreateUserUseCase>();
        handler = new PremiumUserCreatedUserHandler(createUserUseCase);
    }
    
    [Fact]
    public async Task CreatePremiumUser()
    {
        var @event = new PremiumUserCreated{
            Email = "test@example.com"
        };
        
        await handler.Handle(@event);
        
        await createUserUseCase.Received(1).ExecuteAsync(@event.Email, Domain.UserType.Premium);
    }
}