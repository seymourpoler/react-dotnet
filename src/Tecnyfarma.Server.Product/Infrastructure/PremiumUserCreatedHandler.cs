using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Product.Infrastructure;

public class PremiumUserCreatedHandler(CreateUserUseCase createUserUseCase)
{
    public async Task Handle(PremiumUserCreated @event)
    {
        await createUserUseCase.ExecuteAsync(@event.Email, Domain.UserType.Premium);
    }
}