using Microsoft.AspNetCore.Mvc;
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
}