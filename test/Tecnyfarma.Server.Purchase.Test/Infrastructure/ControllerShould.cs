using System.Security.Claims;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Tecnyfarma.Server.Purchase.Application.Purchase;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure;

public class ControllerShould
{
    private readonly CreatePurchaseUseCase useCase;
    private readonly Tecnyfarma.Server.Purchase.Infrastructure.Controller controller;
    
    public ControllerShould()
    {
        useCase = Substitute.For<CreatePurchaseUseCase>(null, null, null);
        controller = new Tecnyfarma.Server.Purchase.Infrastructure.Controller(useCase);
        var httpContext = Substitute.For<HttpContext>();
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact]
    public async Task ReturnErrorWhenUserIsNotLoggedIn()
    {
        useCase.ExecuteAsync(Arg.Any<Args>()).Returns(new Error("User not found"));
        var idProduct = Guid.NewGuid();

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
        
        useCase.ExecuteAsync(Arg.Is<Args>(x => x.Id == idProduct && x.Email == email)).Returns(Either<Error, Unit>.Right(Unit.Default));
        
        var result = await controller.CreatePurchase(idProduct);
        
        Assert.IsType<OkResult>(result);
    }
}