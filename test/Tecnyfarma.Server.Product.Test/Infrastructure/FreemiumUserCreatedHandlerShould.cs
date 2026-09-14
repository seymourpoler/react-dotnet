using NSubstitute;
using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.Product.Domain;
using Tecnyfarma.Server.Product.Infrastructure;
using Tecnyfarma.Server.User.Message;
using Xunit;

namespace Tecnyfarma.Server.Product.Test.Infrastructure;

public class FreemiumUserCreatedHandlerShould
{
    private readonly CreateUserUseCase useCase;
    private readonly FreemiumUserCreatedHandler handler;

    public FreemiumUserCreatedHandlerShould()
    {
        var userRepository = Substitute.For<UserRepository>();
        useCase = Substitute.For<CreateUserUseCase>(userRepository);
        handler = new FreemiumUserCreatedHandler(useCase);
    }

    [Fact]
    public async Task CreateAFreemiumUser()
    {
        var userCreated = new FreemiumUserCreated { Email = "user@example.com" };

        await handler.Handle(userCreated);

        await useCase.Received(1).ExecuteAsync("user@example.com", UserType.Freemium);
        await useCase.DidNotReceive().ExecuteAsync(Arg.Any<string>(), UserType.Premium);
    }

    [Fact]
    public async Task NotThrowWhenUseCaseReturnsAnError()
    {
        useCase.ExecuteAsync(Arg.Any<string>(), Arg.Any<UserType>()).Returns(new Error("Database error"));
        var userCreated = new FreemiumUserCreated { Email = "user@example.com" };

        await handler.Handle(userCreated);

        await useCase.Received(1).ExecuteAsync("user@example.com", UserType.Freemium);
    }
}