using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.Login;
using Tecnyfarma.Server.User.Infrastructure.Login;

namespace Tecnyfarma.Server.Test.User.Infrastructure.Login;

public class LoginControllerShould
{
    private readonly LoginUseCase useCase;
    private readonly LoginController controller;
    
    public LoginControllerShould()
    {
        useCase = Substitute.For<LoginUseCase>(Substitute.For<UserRepository>());
        controller = new LoginController(useCase);
    }
    
    [Fact]
    public async Task ReturnOkWhenLoginSucceeds()
    {
        useCase.Execute(Arg.Any<UseCaseArgs>()).Returns(Unit.Default);
        var request = new LoginRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.Login(request);
        
        result.ShouldBeOfType<OkResult>();
    }
    
    [Fact]
    public async Task ReturnBadRequestWhenRegistrationFails()
    {
        useCase.Execute(Arg.Any<UseCaseArgs>()).Returns(new Error("error message"));
        var request = new LoginRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.Login(request);

        result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.ShouldBe("error message");
    }
}