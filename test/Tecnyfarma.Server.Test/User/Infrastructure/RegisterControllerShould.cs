using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Infrastructure;

namespace Tecnyfarma.Server.Test.User.Infrastructure;

public class RegisterControllerShould
{
    private readonly RegisterUserUseCase useCase;
    private readonly RegisterController controller;

    public RegisterControllerShould()
    {
        useCase = Substitute.For<RegisterUserUseCase>(Substitute.For<UserRepository>());
        controller = new RegisterController(useCase);
    }
    
    [Fact]
    public async Task ReturnOkWhenRegistrationSucceeds()
    {
        useCase.Execute(Arg.Any<RegisterUserArgs>()).Returns(Unit.Default);
        var request = new RegisterUserRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.Register(request);
        
        result.ShouldBeOfType<OkResult>();
    }
    
    [Fact]
    public async Task ReturnBadRequestWhenRegistrationFails()
    {
        useCase.Execute(Arg.Any<RegisterUserArgs>()).Returns(new Error("error message"));
        var request = new RegisterUserRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.Register(request);

        result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.ShouldBe("error message");
    }
}