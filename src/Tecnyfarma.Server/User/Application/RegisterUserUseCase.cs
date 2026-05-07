using LanguageExt;

namespace Tecnyfarma.Server.User.Application;

public class RegisterUserUseCase
{
    private readonly UserRepository repository;
    public RegisterUserUseCase(UserRepository repository)
    {
        this.repository = repository;
    }
    
    public virtual async Task<Either<Error, Unit>> Execute(RegisterUserArgs args)
    {
        var user = new Domain.User(args.Email, args.Password);
        var pp = await (
            from a in repository.SaveAsync(user).ToAsync()
            select Unit.Default
        );
        return pp;
    }
}