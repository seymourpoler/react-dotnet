using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.Register;
using Tecnyfarma.Server.User.Domain;
using Tecnyfarma.Server.User.Infrastructure.Register;
using Controller = Tecnyfarma.Server.User.Infrastructure.Register.Controller;

namespace Tecnyfarma.Server.User.test.Infrastructure.Register;

public class ControllerShould
{
    private readonly UseCase useCase;
    private readonly Controller controller;

    public ControllerShould(){
        useCase = Substitute.For<UseCase>(Substitute.For<UserRepository>());
        controller = new Controller(useCase);
    }


    [Fact]
    public async Task ReturnOkWhenRegistrationSucceeds()
    {
        useCase.Execute(Arg.Any<Args>()).Returns(Task.FromResult<Either<Error, Unit>>(Unit.Default));
        var request = new Request { Email = "user@example.com", Password = "secret123" };

        var result = await controller.Register(request);
        
        result.ShouldBeOfType<OkResult>();
    }
    
    [Fact]
    public async Task ReturnBadRequestWhenRegistrationFails()
    {
        useCase.Execute(Arg.Any<Args>()).Returns(Task.FromResult<Either<Error, Unit>>(new Error("error message")));
        var request = new Request { Email = "user@example.com", Password = "secret123" };

        var result = await controller.Register(request);

        result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.ShouldBe("error message");
    }
}