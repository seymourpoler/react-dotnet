using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
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
        var useCase = A.Fake< RegisterUserUseCase>();
        controller = new RegisterController(useCase);
    }
    
    [Fact]
    public async Task ReturnOkWhenRegistrationSucceeds()
    {
        var request = new RegisterUserRequest { Email = "user@example.com", Password = "secret123" };

        var result = await controller.Register(request);

        result.ShouldBeOfType<OkResult>();
    }
}