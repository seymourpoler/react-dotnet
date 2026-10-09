using NSubstitute;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Infrastructure;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure;

public class PremiumUserCreatedHandlerShould
{
    private readonly CreateUserUseCase createUserUseCase;
    private readonly PremiumUserCreatedHandler handler;

    public PremiumUserCreatedHandlerShould()
    {
        createUserUseCase = Substitute.For<CreateUserUseCase>(Substitute.For<UserRepository>());
        handler = new PremiumUserCreatedHandler(createUserUseCase);
    }
    
    [Fact]
    public async Task CreatePremiumUser()
    {
        var anEvent = new PremiumUserCreated{
            Email = "test@example.com"
        };
        
        await handler.Handle(anEvent);
        
        await createUserUseCase.Received(1).ExecuteAsync(anEvent.Email, Domain.UserType.Premium);
    }
}