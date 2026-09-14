using LanguageExt;
using Tecnyfarma.Server.User.Domain;
using Tecnyfarma.Server.User.Message;
using Wolverine;
using Type = Tecnyfarma.Server.User.Domain.Type;

namespace Tecnyfarma.Server.User.Application.Register;

public class UseCase(Repository repository, IMessageBus bus)
{
    public virtual async Task<Either<Error, Unit>> Execute(Args args)
    {
        return await (
            from email in Email.Create(args.Email).ToAsync()
            from password in Password.Create(args.Password).ToAsync()
            from _ in EnsureThatTheNewUserIsNotAlreadyRegistered(email).ToAsync()
            let user = new Domain.User(email, password)
            from result in repository.SaveAsync(user).ToAsync()
            from __ in PublishUserCreated(user).ToAsync()
            select result
        );
    }

    private async Task<Either<Error, Unit>> EnsureThatTheNewUserIsNotAlreadyRegistered(Email email)
    {
        var found = await repository.FindAsync(email);
        return found.Match<Either<Error, Unit>>(
            _ => new Error("User already registered"),
            _ => Unit.Default
        );
    }
    
    private async Task<Either<Error, Unit>> PublishUserCreated(Domain.User user)
    {
        if(user.Type == Type.Freemium)
        {
            await bus.PublishAsync(new FreemiumUserCreated
            {
                Email = user.Email.Value,
                CreatedAtUtc = user.CreatedAtUtc
            });
            return Either<Error, Unit>.Right(Unit.Default);
        }

        await bus.PublishAsync(new PremiumUserCreated
        {
            Email = user.Email.Value,
            CreatedAtUtc = user.CreatedAtUtc
        });
        return Either<Error, Unit>.Right(Unit.Default);
    }
}