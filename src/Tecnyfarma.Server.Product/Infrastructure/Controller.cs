using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.Product.Application;

namespace Tecnyfarma.Server.Product.Infrastructure;

[ApiController]
public class Controller(UseCase useCase) : ControllerBase
{
    [HttpGet("/api/v0/products")]
    [Authorize]
    public async Task<IActionResult> FindProducts()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var result = await useCase.ExecuteAsync(new Args(email));
        return Ok(result);
    }
}