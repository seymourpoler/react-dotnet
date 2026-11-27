using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Product.Infrastructure.EventHandler;

public class FreemiumUserCreatedHandler(UseCase useCase)
{
    public async Task Handle(FreemiumUserCreated userCreated)
    {
        await useCase.ExecuteAsync(userCreated.Email, Domain.UserType.Freemium);
    }
}