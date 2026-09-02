using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.User.Application.LogIn;

namespace Tecnyfarma.Server.User.Infrastructure.LogIn;

[ApiController]
public class Controller(UseCase useCase) : ControllerBase
{
    [HttpPost("/api/v0/users/login")]
    public async Task<IActionResult> LogIn(Request request)
    {
        var args = new Args(request.Email, request.Password);
        var result = await useCase.Execute(args);
        return await result.MatchAsync<IActionResult>(
            async _ =>
            {
                var identity = new ClaimsIdentity(
                    new [] 
                    {
                        new Claim(ClaimTypes.Email, request.Email)
                    },
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
                return Ok();
            },
            error => BadRequest(error.Message)
        );
    }
}