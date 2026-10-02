using NSubstitute;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Infrastructure;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure;

public class FreemiumUserCreatedHandlerShould
{
    private readonly CreateUserUseCase createUserUseCase;
    private readonly FreemiumUserCreatedHandler handler;

    public FreemiumUserCreatedHandlerShould()
    {
        createUserUseCase = Substitute.For<CreateUserUseCase>();
        handler = new FreemiumUserCreatedHandler(createUserUseCase);
    }
    
    [Fact]
    public async Task CreateFreemiumUser()
    {
        var anEvent = new FreemiumUserCreated{
            Email = "test@example.com"
        };
        
        await handler.Handle(anEvent);
        
        await createUserUseCase.Received(1).ExecuteAsync(anEvent.Email, Domain.UserType.Freemium);
    }
}