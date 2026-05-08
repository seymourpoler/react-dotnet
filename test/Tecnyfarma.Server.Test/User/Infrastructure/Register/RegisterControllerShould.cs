using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.Register;
using Tecnyfarma.Server.User.Infrastructure.Register;

namespace Tecnyfarma.Server.Test.User.Infrastructure.Register;

public class RegisterControllerShould
{
    private readonly RegisterUseCase useCase;
    private readonly RegisterController controller;

    public RegisterControllerShould()
    {
        useCase = Substitute.For<RegisterUseCase>(Substitute.For<UserRepository>());
        controller = new RegisterController(useCase);
    }
    
    [Fact]
    public async Task ReturnOkWhenRegistrationSucceeds()
    {
        useCase.Execute(Arg.Any<UseCaseArgs>()).Returns(Unit.Default);
        var request = new RegisterRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.Register(request);
        
        result.ShouldBeOfType<OkResult>();
    }
    
    [Fact]
    public async Task ReturnBadRequestWhenRegistrationFails()
    {
        useCase.Execute(Arg.Any<UseCaseArgs>()).Returns(new Error("error message"));
        var request = new RegisterRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.Register(request);

        result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.ShouldBe("error message");
    }
}