using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TableReserve.Application.UseCases.User.Login;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;

namespace TableReserve.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(RegisteredUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUser useCase,
        [FromBody] LoginUserRequest request)
    {
        var response = await useCase.Execute(request, HttpContext.RequestAborted);

        return Ok(response);
    }
}
