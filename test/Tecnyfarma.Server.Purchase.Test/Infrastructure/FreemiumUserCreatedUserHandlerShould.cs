using NSubstitute;
using Tecnyfarma.Server.Purchase.Application;
using Tecnyfarma.Server.Purchase.Infrastructure;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure;

public class FreemiumUserCreatedUserHandlerShould
{
    private readonly CreateUserUseCase createUserUseCase;
    private readonly FreemiumUserCreatedUserHandler handler;

    public FreemiumUserCreatedUserHandlerShould()
    {
        createUserUseCase = Substitute.For<CreateUserUseCase>();
        handler = new FreemiumUserCreatedUserHandler(createUserUseCase);
    }
    
    [Fact]
    public async Task CreateFreemiumUser()
    {
        var @event = new FreemiumUserCreated{
            Email = "test@example.com"
        };
        
        await handler.Handle(@event);
        
        await createUserUseCase.Received(1).ExecuteAsync(@event.Email, Domain.UserType.Freemium);
    }
}