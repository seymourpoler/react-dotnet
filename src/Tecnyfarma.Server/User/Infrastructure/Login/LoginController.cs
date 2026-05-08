using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.User.Application.Login;

namespace Tecnyfarma.Server.User.Infrastructure.Login;

[ApiController]
public class LoginController(LoginUseCase useCase) : ControllerBase
{
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var args = new UseCaseArgs(request.Email, request.Password);
        var result = await useCase.Execute(args);
        return result.Match<IActionResult>(
            _ => Ok(),
            error => BadRequest(error.Message)
        );
    }
}