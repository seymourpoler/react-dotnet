using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.Purchase.Application.Purchase;

namespace Tecnyfarma.Server.Purchase.Infrastructure;

[ApiController]
public class Controller(CreatePurchaseUseCase useCase) : ControllerBase
{
    [Authorize]
    [HttpPost("api/v0/purchases")]
    public async Task<IActionResult> CreatePurchase(Request request)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var result = await useCase.ExecuteAsync(request.ProductId, email);

        return result.Match<IActionResult>(
            Right: _ => Ok(),
            Left: error => BadRequest(error.Message)
        );
    }
}