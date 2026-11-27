using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Purchase.Infrastructure.EventHandler;

public class FreemiumUserCreatedHandler(UseCase useCase)
{
    public async Task Handle(FreemiumUserCreated userCreated)
    {
        await useCase.ExecuteAsync(userCreated.Email, Domain.UserType.Freemium);
    }
}