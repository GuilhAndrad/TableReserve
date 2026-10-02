using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TableReserve.Application.UseCases.Reservation.CancelReservation;
using TableReserve.Application.UseCases.Reservation.CreateReservation;
using TableReserve.Application.UseCases.Reservation.ListReservations;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;

namespace TableReserve.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ReservationsController : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateReservation useCase,
        [FromBody] CreateReservationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request, cancellationToken);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReservationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromServices] IListReservations useCase, CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(
        [FromServices] ICancelReservation useCase,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        await useCase.Execute(id, cancellationToken);

        return NoContent();
    }
}