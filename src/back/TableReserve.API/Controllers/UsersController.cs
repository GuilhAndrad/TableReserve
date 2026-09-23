using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TableReserve.Application.UseCases.User.GetUser;
using TableReserve.Application.UseCases.User.RegisterAccount;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;

namespace TableReserve.API.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RegisteredUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUser useCase,
        [FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request, cancellationToken);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUser useCase, CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(cancellationToken);

        return Ok(result);
    }
}