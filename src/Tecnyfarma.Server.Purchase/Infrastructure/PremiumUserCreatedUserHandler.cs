using Tecnyfarma.Server.Purchase.Application;
using Tecnyfarma.Server.User.Message;

namespace Tecnyfarma.Server.Purchase.Infrastructure;

public class PremiumUserCreatedUserHandler(CreateUserUseCase createUserUseCase)
{
    public async Task Handle(PremiumUserCreated userCreated)
    {
        await createUserUseCase.ExecuteAsync(userCreated.Email, Domain.UserType.Premium);
    }
}