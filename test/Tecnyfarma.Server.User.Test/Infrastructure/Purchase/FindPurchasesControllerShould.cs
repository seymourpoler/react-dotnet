using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.User.Application.Purchase;
using Tecnyfarma.Server.User.Application.Purchase.Find;
using Tecnyfarma.Server.User.Domain;
using Tecnyfarma.Server.User.Infrastructure.Purchase;
using Xunit;

namespace Tecnyfarma.Server.User.Test.Infrastructure.Purchase;

public class FindPurchasesControllerShould
{
    private const string email = "a@mail.es";

    private readonly UseCase useCase;
    private readonly FindPurchasesController controller;

    public FindPurchasesControllerShould()
    {
        useCase = Substitute.For<UseCase>(Substitute.For<PurchaseRepository>());
        controller = new FindPurchasesController(useCase);
        var httpContext = Substitute.For<HttpContext>();
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "test");
        var user = new ClaimsPrincipal(identity);
        controller.HttpContext!.User.Returns(user);
    }

    [Fact]
    public async Task FindPurchases()
    {
        var purchases = Array.Empty<Domain.Purchase>();
        useCase.ExecuteAsync(email).Returns(purchases);

        var result = await controller.FindPuchases();

        result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ReturnErrorWhenSomethingGoesWrong()
    {
        useCase.ExecuteAsync(email).Returns(new Error("There is something wrong"));

        var result = await controller.FindPuchases();

        result.ShouldBeOfType<BadRequestObjectResult>();
    }
}