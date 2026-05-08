using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.Register;

namespace Tecnyfarma.Server.Test.User.Application.Register;

public class RegisterUserUseCaseShould
{
    private readonly UserRepository repository;
    private readonly RegisterUseCase useCase;

    public RegisterUserUseCaseShould()
    {
        repository = Substitute.For<UserRepository>();
        useCase = new RegisterUseCase(repository);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    public async Task ReturnErrorWhenEmailIsInvalid(string email)
    {
        var args = new UseCaseArgs(email, "a-password");
        
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
    public async Task ReturnErrorWhenRepositoryFails()
    {
        var args = new UseCaseArgs("user@example.com", "validpassword");
        repository.SaveAsync(Arg.Any<Server.User.Domain.User>())
            .Returns(new Error("User already exists"));

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("User already exists")
        );
    }
    
    [Fact]
    public async Task ReturnSuccessWhenRegistrationIsValid()
    {
        var args = new UseCaseArgs("user@example.com", "valid-password");
        repository.SaveAsync(Arg.Is<Server.User.Domain.User>(x => x.Email.Value == "user@example.com"))
            .Returns(Unit.Default);

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.True(true, "Expected an success but got an error result"),
            error => Assert.Fail($"Expected success but got error: {error.Message}")
        );
    }
}