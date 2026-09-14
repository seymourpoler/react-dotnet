using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.Product.Application.Poduct;

namespace Tecnyfarma.Server.Product.Infrastructure;

[ApiController]
public class Controller(FindProductsUseCase findProductsUseCase) : ControllerBase
{
    [HttpGet("/api/v0/products")]
    public async Task<IActionResult> FindProducts()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var result = await findProductsUseCase.ExecuteAsync(new Args(email));
        return Ok(result);
    }
}