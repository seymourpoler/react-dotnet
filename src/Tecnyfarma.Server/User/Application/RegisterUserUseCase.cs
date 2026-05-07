namespace Tecnyfarma.Server.User.Application;

public class RegisterUserUseCase(UserRepository repository)
{
    public async Task Execute(RegisterUserArgs args)
    {
        var user = new Domain.User(args.Email, args.Password);
        await repository.SaveAsync(user);
    }
}