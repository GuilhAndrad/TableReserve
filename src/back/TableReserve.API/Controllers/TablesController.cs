using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TableReserve.Application.UseCases.Table.CreateTable;
using TableReserve.Application.UseCases.Table.ListTables;
using TableReserve.Application.UseCases.Table.UpdateTable;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;

namespace TableReserve.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class TablesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<TableResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromServices] IListTables useCase, CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(cancellationToken);

        return Ok(result);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(TableResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateTable useCase,
        [FromBody] CreateTableRequest request,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request, cancellationToken);

        return Created(string.Empty, result);
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(TableResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateTable useCase,
        [FromRoute] Guid id,
        [FromBody] UpdateTableRequest request,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(id, request, cancellationToken);

        return Ok(result);
    }
}