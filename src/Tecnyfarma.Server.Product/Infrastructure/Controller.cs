using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.Product.Application.Product.Create;
using Tecnyfarma.Server.Product.Application.Product.Find;
using UseCase = Tecnyfarma.Server.Product.Application.Product.Find.UseCase;

namespace Tecnyfarma.Server.Product.Infrastructure;

[ApiController]
public class Controller(
    Tecnyfarma.Server.Product.Application.Product.Find.UseCase findProductUseCase,
    Tecnyfarma.Server.Product.Application.Product.Create.UseCase createProductUseCase) : ControllerBase
{
    [HttpGet("/api/v0/products")]
    public async Task<IActionResult> FindProducts()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var args = new Tecnyfarma.Server.Product.Application.Product.Find.Args(email);
        var result = await findProductUseCase.ExecuteAsync(args);
        
        return result.Match<IActionResult>(
            products => Ok(products),
            error => BadRequest(error.Message)
        );
    }

    [HttpPost("/api/v0/products")]
    public async Task<IActionResult> Create(Request request)
    {
        var args = new Tecnyfarma.Server.Product.Application.Product.Create.Args(request.Name, request.Description, request.Price);
        var result = await createProductUseCase.ExecuteAsync(args);
        
        return result.Match<IActionResult>(
            _ => Ok(),
            error => BadRequest(error.Message)
        );
    }
}