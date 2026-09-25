using Microsoft.AspNetCore.Mvc;

namespace Tecnyfarma.Server.Purchase.Infrastructure;

[ApiController]
public class PurchaseController : ControllerBase
{
    [HttpPost("api/v0/purchases")]
    public async Task<IActionResult> CreatePurchase()
    {
        throw new NotImplementedException();
    }
}