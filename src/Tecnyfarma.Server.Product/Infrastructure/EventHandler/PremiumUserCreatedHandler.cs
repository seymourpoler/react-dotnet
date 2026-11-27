using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Product.Infrastructure.EventHandler;

public class PremiumUserCreatedHandler(UseCase useCase)
{
    public async Task Handle(PremiumUserCreated @event)
    {
        await useCase.ExecuteAsync(@event.Email, Domain.UserType.Premium);
    }
}