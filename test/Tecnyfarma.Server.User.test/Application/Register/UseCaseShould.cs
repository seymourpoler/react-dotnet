using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.Register;
using Tecnyfarma.Server.User.Domain;
using Xunit;

namespace Tecnyfarma.Server.User.test.Application.Register;

public class UseCaseShould
{
    private readonly UserRepository repository;
    private readonly UseCase useCase;

    public UseCaseShould()
    {
        repository = Substitute.For<UserRepository>();
        useCase = new UseCase(repository);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    public async Task ReturnErrorWhenEmailIsInvalid(string email)
    {
        var args = new Args(email, "a-password");
        
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
        var args = new Args("user@example.com", password);
        
        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("Password must be at least 6 characters long")
        );
    }

    [Fact]
    public async Task ReturnErrorWhenRepositoryFails()
    {
        var args = new Args("user@example.com", "validpassword");
        repository.FindAsync(Arg.Any<Email>()).Returns(new Error("User not found"));
        repository.SaveAsync(Arg.Any<Server.User.Domain.User>())
            .Returns(new Error("Database error"));

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("Database error")
        );
    }
    
    [Fact]
    public async Task ReturnErrorWhenUserIsAlreadyRegistered()
    {
        var email = Email.Create("user@example.com").Match(Right: x => x, Left: _ => throw new Exception());
        var password = Password.Create("valid-password").Match(Right: x => x, Left: _ => throw new Exception());
        repository.FindAsync(Arg.Any<Email>()).Returns(new Server.User.Domain.User(email, password));

        var args = new Args("user@example.com", "valid-password");

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("User already registered")
        );
        await repository.DidNotReceive().SaveAsync(Arg.Any<Server.User.Domain.User>());
    }

    [Fact]
    public async Task ReturnSuccessWhenRegistrationIsValid()
    {
        var args = new Args("user@example.com", "valid-password");
        repository.FindAsync(Arg.Any<Email>()).Returns(new Error("User not found"));
        repository.SaveAsync(Arg.Is<Server.User.Domain.User>(x => x.Email.Value == "user@example.com"))
            .Returns(Unit.Default);

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.True(true, "Expected an success but got an error result"),
            error => Assert.Fail($"Expected success but got error: {error.Message}")
        );
    }
}