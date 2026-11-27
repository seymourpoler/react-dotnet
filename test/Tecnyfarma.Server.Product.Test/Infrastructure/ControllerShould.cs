using System.Security.Claims;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.Product.Application.Product;
using Tecnyfarma.Server.Product.Application.Product.Create;
using Tecnyfarma.Server.Product.Application.Product.Find;
using Tecnyfarma.Server.Product.Domain;
using Tecnyfarma.Server.Product.Infrastructure;
using Wolverine;
using Xunit;

namespace Tecnyfarma.Server.Product.Test.Infrastructure;

public class ControllerShould
{
    private readonly FindProductsUseCase findProducts;
    private readonly CreateProductUseCase createProduct;
    private readonly Tecnyfarma.Server.Product.Infrastructure.Controller controller;

    public ControllerShould()
    {
        findProducts = Substitute.For<FindProductsUseCase>(null, null);
        createProduct = Substitute.For<CreateProductUseCase>(Substitute.For<ProductRepository>(), Substitute.For<IMessageBus>());
        controller = new Tecnyfarma.Server.Product.Infrastructure.Controller(findProducts, createProduct);
        var httpContext = Substitute.For<HttpContext>();
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact]
    public async Task ReturnProductsWhenUserIsNotLogged()
    {
        findProducts.ExecuteAsync(Arg.Any<Tecnyfarma.Server.Product.Application.Product.Find.Args>()).Returns(new List<Tecnyfarma.Server.Product.Domain.Product>());

        var result = await controller.FindProducts();

        result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ReturnProductsWhenUserIsLogged()
    {
        findProducts.ExecuteAsync(Arg.Any<Tecnyfarma.Server.Product.Application.Product.Find.Args>()).Returns(new List<Tecnyfarma.Server.Product.Domain.Product>());
        SetLoggedUser("e@mail.com");

        var result = await controller.FindProducts();

        await findProducts.Received().ExecuteAsync(Arg.Is<Tecnyfarma.Server.Product.Application.Product.Find.Args>(args => args.Email == "e@mail.com"));
        result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ReturnBadRequestWhenThereIsAndErrorWhenFindingProducts()
    {
        findProducts.ExecuteAsync(Arg.Any<Tecnyfarma.Server.Product.Application.Product.Find.Args>()).Returns(new Tecnyfarma.Server.Product.Domain.Error("error"));

        var result = await controller.FindProducts();

        result.ShouldBeOfType<BadRequestObjectResult>();
    }


    private void SetLoggedUser(string email)
    {
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "test");
        var user = new ClaimsPrincipal(identity);
        controller.HttpContext!.User.Returns(user);
    }

    [Fact]
    public async Task ReturnCreatedProduct()
    {
        createProduct.ExecuteAsync(Arg.Any<Tecnyfarma.Server.Product.Application.Product.Create.Args>()).Returns(Unit.Default);
        var request = new Request { Name = "a-name", Description = "a-description", Price = 10.5f };


        var result = await controller.Create(request);


        result.ShouldBeOfType<OkResult>();
    }

    [Fact]
    public async Task ReturnErrorWhenThereIsAnErrorOnCreatingProduct()
    {
        createProduct.ExecuteAsync(Arg.Any<Tecnyfarma.Server.Product.Application.Product.Create.Args>()).Returns(new Error("There is an error"));
        var request = new Request { Name = "a-name", Description = "a-description", Price = 10.5f };


        var result = await controller.Create(request);


        result.ShouldBeOfType<BadRequestObjectResult>();
    }
}
