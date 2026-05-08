using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.Login;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.Test.User.Application.Login;

public class LoginUseCaseShould
{
    private readonly UserRepository repository;
    private readonly LoginUseCase useCase;

    public LoginUseCaseShould()
    {
        repository = Substitute.For<UserRepository>();
        useCase = new LoginUseCase(repository);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    public async Task ReturnErrorWhenEmailIsInvalid(string email)
    {
        var args = new UseCaseArgs(email, "validpassword");

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("Invalid email")
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123")]
    public async Task ReturnErrorWhenPasswordIsInvalid(string password)
    {
        var args = new UseCaseArgs("user@example.com", password);

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("Password must be at least 6 characters long")
        );
    }

    [Fact]
    public async Task ReturnErrorWhenUserNotFound()
    {
        var args = new UseCaseArgs("user@example.com", "validpassword");
        repository.FindAsync(Arg.Any<Email>())
            .Returns(new Error("User not found"));

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("User not found")
        );
    }

    [Fact]
    public async Task ReturnErrorWhenPasswordDoesNotMatch()
    {
        var email = Email.Create("user@example.com").Match(Right: x => x, Left: _ => throw new Exception());
        var password = Password.Create("a-password").Match(Right: x => x, Left: _ => throw new Exception());
        var storedUser = new Server.User.Domain.User(email, password);
        repository.FindAsync(Arg.Any<Email>()).Returns(storedUser);
        var args = new UseCaseArgs("user@example.com", "wrongpassword!");

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("Invalid credentials")
        );
    }

    [Fact]
    public async Task ReturnSuccessWhenCredentialsAreValid()
    {
        var email = Email.Create("user@example.com").Match(Right: x => x, Left: _ => throw new Exception());
        var password = Password.Create("a-password").Match(Right: x => x, Left: _ => throw new Exception());
        var storedUser = new Server.User.Domain.User(email, password);

        repository.FindAsync(Arg.Any<Email>())
            .Returns(Task.FromResult<Either<Error, Server.User.Domain.User>>(storedUser));

        var args = new UseCaseArgs("user@example.com", "a-password");

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.True(true, "Expected success but got error result"),
            error => Assert.Fail($"Expected success but got error: {error.Message}")
        );
    }
}