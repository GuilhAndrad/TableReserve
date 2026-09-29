using Mapster;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Enums;
using TableReserve.Domain.Repositories;
using TableReserve.Domain.Repositories.Table;
using TableReserve.Domain.Repositories.User;
using TableReserve.Exception;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.Application.UseCases.Table.UpdateTable;

public class UpdateTable : IUpdateTable
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITableReader _tableReader;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTable(ILoggedUser loggedUser, ITableReader tableReader, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _tableReader = tableReader;
        _unitOfWork = unitOfWork;
    }

    public async Task<TableResponse> Execute(Guid id, UpdateTableRequest request, CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.Get(cancellationToken);
        if (loggedUser.Role != UserRole.Administrator)
            throw new ForbiddenException();

        Validate(request);

        var table = await _tableReader.GetById(id, cancellationToken)
            ?? throw new NotFoundException(MessagesExceptionResource.TABLE_NOT_FOUND_VALIDATION);

        table.Name = request.Name;
        table.Capacity = request.Capacity;
        table.Status = request.Status.Adapt<Domain.Enums.TableStatus>();

        await _unitOfWork.Commit(cancellationToken);

        return new TableResponse
        {
            Id = table.Id,
            Name = table.Name,
            Capacity = table.Capacity,
            Status = table.Status.Adapt<Communication.Enums.TableStatus>()
        };
    }

    private static void Validate(UpdateTableRequest request)
    {
        var validator = new UpdateTableValidator();
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidationException(errorMessages);
        }
    }
}