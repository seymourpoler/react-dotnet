using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.User.Application.Register;

namespace Tecnyfarma.Server.User.Infrastructure.Register;

[ApiController]
public class RegisterController(RegisterUseCase useCase) : ControllerBase
{
    [HttpPost("/api/v0/register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var args = new UseCaseArgs(request.Email, request.Password);
        var result = await useCase.Execute(args);

        return result.Match<IActionResult>(
            _ => Ok(),
            error => BadRequest(error.Message)
        );
    }
}