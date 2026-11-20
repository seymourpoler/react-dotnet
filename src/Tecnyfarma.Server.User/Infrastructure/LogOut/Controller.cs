using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tecnyfarma.Server.User.Infrastructure.LogOut;

[ApiController]
public class Controller : ControllerBase
{
    [Authorize]
    [HttpPost("/api/v0/users/logout")]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}