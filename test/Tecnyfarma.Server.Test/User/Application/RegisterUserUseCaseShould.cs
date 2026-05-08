using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;

namespace Tecnyfarma.Server.Test.User.Application;

public class RegisterUserUseCaseShould
{
    private readonly UserRepository repository;
    private readonly RegisterUserUseCase useCase;

    public RegisterUserUseCaseShould()
    {
        repository = Substitute.For<UserRepository>();
        useCase = new RegisterUserUseCase(repository);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    public async Task ReturnErrorWhenEmailIsInvalid(string email)
    {
        var args = new RegisterUserArgs(email, "a-password");
        
        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error =>
            {
                error.Message.ShouldBe("Invalid email");
            });
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123")]
    public async Task ReturnErrorWhenPasswordIsInvalid(string password)
    {
        var args = new RegisterUserArgs("user@example.com", password);
        
        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("Password must be at least 6 characters long")
        );
    }

    [Fact]
    public async Task ReturnSuccessWhenRegistrationIsValid()
    {
        var args = new RegisterUserArgs("user@example.com", "valid-password");
        repository.SaveAsync(Arg.Any<Tecnyfarma.Server.User.Domain.User>())
            .Returns(Unit.Default);

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.True(true, "Expected an success but got an error result"),
            error => Assert.Fail($"Expected success but got error: {error.Message}")
        );
        await repository.Received(1).SaveAsync(Arg.Any<Tecnyfarma.Server.User.Domain.User>());
    }

    [Fact]
    public async Task ReturnErrorWhenRepositoryFails()
    {
        var args = new RegisterUserArgs("user@example.com", "validpassword");
        repository.SaveAsync(Arg.Any<Tecnyfarma.Server.User.Domain.User>())
            .Returns(new Error("User already exists"));

        var result = await useCase.Execute(args);
        
        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("User already exists")
        );
        await repository.Received().SaveAsync(Arg.Any<Tecnyfarma.Server.User.Domain.User>());
    }
}