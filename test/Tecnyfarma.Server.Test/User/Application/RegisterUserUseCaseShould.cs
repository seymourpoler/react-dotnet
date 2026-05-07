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
    
    [Fact]
    public async Task ReturnErrorWhenEmailIsInvalid()
    {
        var args = new RegisterUserArgs("invalid-email", "a-password");
        
        var result = await useCase.Execute(args);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error =>
            {
               error.Message.ShouldBe("Invalid email");
            });
    }
}