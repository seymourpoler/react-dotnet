using NSubstitute;
using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.Product.Domain;
using Tecnyfarma.Server.Product.Infrastructure;
using Tecnyfarma.Server.User.Message;
using Xunit;

namespace Tecnyfarma.Server.Product.Test.Infrastructure;

public class PremiumUserCreatedHandlerShould
{
    private readonly CreateUserUseCase useCase;
    private readonly PremiumUserCreatedHandler handler;

    public PremiumUserCreatedHandlerShould()
    {
        var userRepository = Substitute.For<UserRepository>();
        useCase = Substitute.For<CreateUserUseCase>(userRepository);
        handler = new PremiumUserCreatedHandler(useCase);
    }

    [Fact]
    public async Task CreateAPremiumUser()
    {
        var userCreated = new PremiumUserCreated { Email = "user@example.com" };

        await handler.Handle(userCreated);

        await useCase.Received(1).ExecuteAsync("user@example.com", UserType.Premium);
        await useCase.DidNotReceive().ExecuteAsync(Arg.Any<string>(), UserType.Freemium);
    }

    [Fact]
    public async Task NotThrowWhenUseCaseReturnsAnError()
    {
        useCase.ExecuteAsync(Arg.Any<string>(), Arg.Any<UserType>()).Returns(new Error("Database error"));
        var userCreated = new PremiumUserCreated { Email = "user@example.com" };

        await handler.Handle(userCreated);

        await useCase.Received(1).ExecuteAsync("user@example.com", UserType.Premium);
    }
}