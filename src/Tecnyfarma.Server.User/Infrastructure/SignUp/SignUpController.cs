using Tecnyfarma.Server.User.Application.SignUp;
using Microsoft.AspNetCore.Mvc;

namespace Tecnyfarma.Server.User.Infrastructure.SignUp;

[ApiController]
public class SignUpController(SignUpUseCase useCase) : ControllerBase
{

    [HttpPost("/api/v0/user/signup")]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        var args = new UseCaseArgs(request.Email, request.Password);
        var result = await useCase.Execute(args);

        return result.Match<IActionResult>(
            _ => Ok(),
            error => BadRequest(error.Message)
        );
    }
}