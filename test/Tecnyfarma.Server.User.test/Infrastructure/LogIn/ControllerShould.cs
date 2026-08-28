using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.LogIn;
using Tecnyfarma.Server.User.Domain;
using Tecnyfarma.Server.User.Infrastructure.LogIn;
using Controller = Tecnyfarma.Server.User.Infrastructure.LogIn.Controller;

namespace Tecnyfarma.Server.User.test.Infrastructure.SignIn;

public class ControllerShould
{
    private readonly UseCase useCase;
    private readonly Controller controller;

    public ControllerShould()
    {
        useCase = Substitute.For<UseCase>(Substitute.For<UserRepository>());
        controller = new Controller(useCase);
    }
    
    [Fact]
    public async Task ReturnOkWhenLoginSucceeds()
    {
        useCase.Execute(Arg.Any<Args>()).Returns(Unit.Default);
        var request = new Request { Email = "user@example.com", Password = "secret123" };

        var result = await controller.SignIn(request);
        
        result.ShouldBeOfType<OkResult>();
    }
    
    [Fact]
    public async Task ReturnBadRequestWhenRegistrationFails()
    {
        useCase.Execute(Arg.Any<Args>()).Returns(new Error("error message"));
        var request = new Request { Email = "user@example.com", Password = "secret123" };

        var result = await controller.SignIn(request);

        result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.ShouldBe("error message");
    }
}