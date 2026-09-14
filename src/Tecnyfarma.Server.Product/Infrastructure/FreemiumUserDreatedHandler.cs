using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Product.Infrastructure;

public class FreemiumUserCreatedHandler(CreateUserUseCase createUserUseCase)
{
    public async Task Handle(FreemiumUserCreated userCreated)
    {
        await createUserUseCase.ExecuteAsync(userCreated.Email, Domain.UserType.Freemium);
    }
}