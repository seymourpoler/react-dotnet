using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.User.Application;

namespace Tecnyfarma.Server.User.Infrastructure;

public class RegisterController(RegisterUserUseCase useCase) : ControllerBase
{
    [HttpPost("/api/v0/register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var args = new RegisterUserArgs(request.Email, request.Password);
        await useCase.Execute(args);
        return Ok();
    }
}