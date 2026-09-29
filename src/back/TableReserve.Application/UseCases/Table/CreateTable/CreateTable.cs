using Mapster;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Enums;
using TableReserve.Domain.Repositories;
using TableReserve.Domain.Repositories.Table;
using TableReserve.Domain.Repositories.User;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.Application.UseCases.Table.CreateTable;

public class CreateTable : ICreateTable
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITableWriter _tableWriter;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTable(ILoggedUser loggedUser, ITableWriter tableWriter, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _tableWriter = tableWriter;
        _unitOfWork = unitOfWork;
    }

    public async Task<TableResponse> Execute(CreateTableRequest request, CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.Get(cancellationToken);
        if (loggedUser.Role != UserRole.Administrator)
            throw new ForbiddenException();

        Validate(request);

        var table = new Domain.Entities.Table
        {
            Name = request.Name,
            Capacity = request.Capacity
        };

        await _tableWriter.Add(table, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return new TableResponse
        {
            Id = table.Id,
            Name = table.Name,
            Capacity = table.Capacity,
            Status = table.Status.Adapt<Communication.Enums.TableStatus>()
        };
    }

    private static void Validate(CreateTableRequest request)
    {
        var validator = new CreateTableValidator();
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidationException(errorMessages);
        }
    }
}