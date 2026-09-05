using Microsoft.AspNetCore.Mvc;

namespace Tecnyfarma.Server.Product;

[ApiController]
public class Controller : ControllerBase
{
    [HttpGet("/api/v0/products")]
    public async Task<IActionResult> FindProducts()
    {
        throw new NotImplementedException();
    }
}