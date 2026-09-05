using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application.Register;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.test.Application.Register;

public class UseCaseShould
{
    private readonly SaveUserRepository repository;
    private readonly Server.User.Application.LogIn.FindUserRepository findUserRepository;
    private readonly UseCase useCase;

    public UseCaseShould()
    {
        repository = Substitute.For<SaveUserRepository>();
        findUserRepository = Substitute.For<Server.User.Application.LogIn.FindUserRepository>();
        findUserRepository.FindAsync(Arg.Any<Email>())
            .Returns(new Error("User not found"));
        useCase = new UseCase(repository, findUserRepository);
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
        repository.SaveAsync(Arg.Any<Server.User.Domain.User>())
            .Returns(new Error("User already exists"));

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("User already exists")
        );
    }
    
    [Fact]
    public async Task ReturnErrorWhenUserIsAlreadyRegistered()
    {
        var email = Email.Create("user@example.com").Match(Right: x => x, Left: _ => throw new Exception());
        var password = Password.Create("valid-password").Match(Right: x => x, Left: _ => throw new Exception());
        findUserRepository.FindAsync(Arg.Any<Email>())
            .Returns(new Server.User.Domain.User(email, password));

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
        repository.SaveAsync(Arg.Is<Server.User.Domain.User>(x => x.Email.Value == "user@example.com"))
            .Returns(Unit.Default);

        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.True(true, "Expected an success but got an error result"),
            error => Assert.Fail($"Expected success but got error: {error.Message}")
        );
    }
        
}