using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.User.Application.SigIn;

namespace Tecnyfarma.Server.User.Infrastructure.SignIn;

[ApiController]
public class SignInController(SignInUseCase useCase) : ControllerBase
{
    [HttpPost("/api/v0/users/signin")]
    public async Task<IActionResult> SignIn(SignInRequest request)
    {
        var args = new UseCaseArgs(request.Email, request.Password);
        var result = await useCase.Execute(args);
        return result.Match<IActionResult>(
            _ => Ok(),
            error => BadRequest(error.Message)
        );
    }
}