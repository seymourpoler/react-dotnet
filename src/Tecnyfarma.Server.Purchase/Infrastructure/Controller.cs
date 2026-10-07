using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.Purchase.Application.Purchase;

namespace Tecnyfarma.Server.Purchase.Infrastructure;

[ApiController]
public class Controller : ControllerBase
{
    private readonly CreatePurchaseUseCase useCase;

    public Controller(CreatePurchaseUseCase useCase)
    {
        this.useCase = useCase;
    }

    [HttpPost("api/v0/purchases")]
    public async Task<IActionResult> CreatePurchase(Guid idProduct)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var result = await useCase.ExecuteAsync(idProduct, email);

        return result.Match<IActionResult>(
            Right: _ => Ok(),
            Left: error => BadRequest(error.Message)
        );
    }
}