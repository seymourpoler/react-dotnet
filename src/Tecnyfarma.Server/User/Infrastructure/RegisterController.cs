using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.User.Application;

namespace Tecnyfarma.Server.User.Infrastructure;

[ApiController]
public class RegisterController(RegisterUserUseCase useCase) : ControllerBase
{
    [HttpPost("/api/v0/register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var args = new RegisterUserArgs(request.Email, request.Password);
        var result = await useCase.Execute(args);

        return result.Match<IActionResult>(
            _ => Ok(),
            error => BadRequest(error.Message)
        );
    }
}