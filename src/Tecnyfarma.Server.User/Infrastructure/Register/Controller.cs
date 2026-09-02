using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.User.Application.Register;

namespace Tecnyfarma.Server.User.Infrastructure.Register;

[ApiController]
public class Controller(UseCase useCase) : ControllerBase
{

    [HttpPost("/api/v0/users/register")]
    public async Task<IActionResult> Register(Request request)
    {
        var args = new Args(request.Email, request.Password);
        var result = await useCase.Execute(args);

        return result.Match<IActionResult>(
            _ => Ok(),
            error => BadRequest(error.Message)
        );
    }
}