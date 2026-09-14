using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.Product.Application.Poduct;
using Xunit;
using Controller = Tecnyfarma.Server.Product.Infrastructure.Controller;

namespace Tecnyfarma.Server.Product.Test.Infrastructure;

public class ControllerShould
{
    private readonly FindProductsUseCase _findProductsUseCase;
    private readonly Controller controller;

    public ControllerShould()
    {
        _findProductsUseCase = Substitute.For<FindProductsUseCase>(null, null);
        controller = new Controller(_findProductsUseCase);
        var httpContext = Substitute.For<HttpContext>();
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact]
    public async Task ReturnProductsWhenUserIsNotLogged()
    {
        _findProductsUseCase.ExecuteAsync(Arg.Any<Args>()).Returns(new List<Tecnyfarma.Server.Product.Domain.Product>());

        var result = await controller.FindProducts();

        result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ReturnProductsWhenUserIsLogged()
    {
        _findProductsUseCase.ExecuteAsync(Arg.Any<Args>()).Returns(new List<Tecnyfarma.Server.Product.Domain.Product>());
        SetLoggedUser("e@mail.com");

        var result = await controller.FindProducts();

        await _findProductsUseCase.Received().ExecuteAsync(Arg.Is<Args>(args => args.Email == "e@mail.com"));
        result.ShouldBeOfType<OkObjectResult>();
    }

    private void SetLoggedUser(string email)
    {
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "test");
        var user = new ClaimsPrincipal(identity);
        controller.HttpContext!.User.Returns(user);
    }
}
