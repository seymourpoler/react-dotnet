using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Purchase.Infrastructure.EventHandler;

public class PremiumUserCreatedHandler(UseCase useCase)
{
    public async Task Handle(PremiumUserCreated userCreated)
    {
        await useCase.ExecuteAsync(userCreated.Email, Domain.UserType.Premium);
    }
}