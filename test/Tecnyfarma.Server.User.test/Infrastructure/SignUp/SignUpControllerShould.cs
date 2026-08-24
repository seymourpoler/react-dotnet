using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Application.SignUp;
using Tecnyfarma.Server.User.Infrastructure.SignUp;

namespace Tecnyfarma.Server.User.test.Infrastructure.SignUp;

public class SignUpControllerShould
{
    private readonly SignUpUseCase useCase;
    private readonly SignUpController controller;

    public SignUpControllerShould(){
        useCase = Substitute.For<SignUpUseCase>(Substitute.For<UserRepository>());
        controller = new SignUpController(useCase);
    }


    [Fact]
    public async Task ReturnOkWhenRegistrationSucceeds()
    {
        useCase.Execute(Arg.Any<UseCaseArgs>()).Returns(Task.FromResult<Either<Error, Unit>>(Unit.Default));
        var request = new SignUpRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.SignUp(request);
        
        result.ShouldBeOfType<OkResult>();
    }
    
    [Fact]
    public async Task ReturnBadRequestWhenRegistrationFails()
    {
        useCase.Execute(Arg.Any<UseCaseArgs>()).Returns(Task.FromResult<Either<Error, Unit>>(new Error("error message")));
        var request = new SignUpRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.SignUp(request);

        result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.ShouldBe("error message");
    }
}