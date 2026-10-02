using System.Security.Claims;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Tecnyfarma.Server.Purchase.Application.Purchase;
using Tecnyfarma.Server.Purchase.Domain;
using Controller = Tecnyfarma.Server.Purchase.Infrastructure.Controller;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure;

public class ControllerShould
{
    private readonly CreatePurchaseUseCase useCase;
    private readonly Controller controller;
    
    public ControllerShould()
    {
        useCase = Substitute.For<CreatePurchaseUseCase>(null, null, null);
        controller = new Controller(useCase);
        var httpContext = Substitute.For<HttpContext>();
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact]
    public async Task ReturnErrorWhenUserIsNotLoggedIn()
    {
        var idProduct = Guid.NewGuid();
        useCase.ExecuteAsync(idProduct, Arg.Any<string>()).Returns(new Error("User not found"));

        var result = await controller.CreatePurchase(idProduct);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task CreatePurchase()
    {
        var idProduct = Guid.NewGuid();
        const string email = "test@example.com";
        
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "test");
        var user = new ClaimsPrincipal(identity);
        controller.HttpContext!.User.Returns(user);
        
        useCase.ExecuteAsync(idProduct, email).Returns(Either<Error, Unit>.Right(Unit.Default));
        
        var result = await controller.CreatePurchase(idProduct);
        
        Assert.IsType<OkResult>(result);
    }
}