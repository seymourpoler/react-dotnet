using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.Product.Application;
using Xunit;
using Controller = Tecnyfarma.Server.Product.Infrastructure.Controller;

namespace Tecnyfarma.Server.Product.Test.Infrastructure;

public class ControllerShould
{
    private readonly UseCase useCase;
    private readonly Controller controller;

    public ControllerShould()
    {
        useCase = Substitute.For<UseCase>(Substitute.For<Repository>());
        controller = new Controller(useCase);
        var httpContext = Substitute.For<HttpContext>();
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact]
    public async Task ReturnProductsWhenUserIsNotLogged()
    {
        useCase.ExecuteAsync(Arg.Any<Args>()).Returns(Task.FromResult(new Result()));

        var result = await controller.FindProducts();

        result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ReturnProductsWhenUserIsLogged()
    {
        useCase.ExecuteAsync(Arg.Any<Args>()).Returns(Task.FromResult(new Result()));
        SetLoggedUser("e@mail.com");

        var result = await controller.FindProducts();

        await useCase.Received().ExecuteAsync(Arg.Is<Args>(args => args.Email == "e@mail.com"));
        result.ShouldBeOfType<OkObjectResult>();
    }

    private void SetLoggedUser(string email)
    {
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "test");
        var user = new ClaimsPrincipal(identity);
        controller.HttpContext!.User.Returns(user);
    }
}
