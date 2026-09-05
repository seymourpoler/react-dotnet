using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Controller = Tecnyfarma.Server.User.Infrastructure.LogOut.Controller;

namespace Tecnyfarma.Server.User.test.Infrastructure.LogOut;

public class ControllerShould
{
    private readonly Controller controller;
    private readonly IAuthenticationService authenticationService;

    public ControllerShould()
    {
        controller = new Controller();
        var httpContext = Substitute.For<HttpContext>();
        var serviceProvider = Substitute.For<IServiceProvider>();
        authenticationService = Substitute.For<IAuthenticationService>();
        serviceProvider.GetService(typeof(IAuthenticationService)).Returns(authenticationService);
        httpContext.RequestServices.Returns(serviceProvider);
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact]
    public async Task LoggingOut()
    {
        var result = await controller.LogOut();

        result.ShouldBeOfType<OkResult>();
        await authenticationService.Received().SignOutAsync(
            Arg.Any<HttpContext>(),
            CookieAuthenticationDefaults.AuthenticationScheme,
            Arg.Any<AuthenticationProperties>());
    }
}
