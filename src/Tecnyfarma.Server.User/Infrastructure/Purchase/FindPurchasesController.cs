using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.User.Application.Purchase;

namespace Tecnyfarma.Server.User.Infrastructure.Purchase;

[ApiController]
public class FindPurchasesController(FindPurchasesUseCase useCase) : ControllerBase
{
    [Authorize]
    [HttpGet("/api/v0/users/purchases")]
    public async Task<IActionResult> FindPuchases()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value; 
        
        var result = await useCase.ExecuteAsync(email);

        return result.Match<IActionResult>(
            Right: purchaes => Ok(purchaes),
            Left: error => BadRequest(error.Message)
        );
    }
}