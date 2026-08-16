using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.SigIn;
using Tecnyfarma.Server.User.Infrastructure.SignIn;

namespace Tecnyfarma.Server.User.test.Infrastructure.SignIn;

public class SignInControllerShould
{
    private readonly SignInUseCase useCase;
    private readonly SignInController controller;

    public SignInControllerShould()
    {
        useCase = Substitute.For<SignInUseCase>(Substitute.For<UserRepository>());
        controller = new SignInController(useCase);
    }
    
    [Fact]
    public async Task ReturnOkWhenLoginSucceeds()
    {
        useCase.Execute(Arg.Any<UseCaseArgs>()).Returns(Unit.Default);
        var request = new SignInRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.SignIn(request);
        
        result.ShouldBeOfType<OkResult>();
    }
    
    [Fact]
    public async Task ReturnBadRequestWhenRegistrationFails()
    {
        useCase.Execute(Arg.Any<UseCaseArgs>()).Returns(new Error("error message"));
        var request = new SignInRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.SignIn(request);

        result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.ShouldBe("error message");
    }
}