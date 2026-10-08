using Mapster;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Repositories;
using TableReserve.Domain.Repositories.Table;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.Application.UseCases.Table.CreateTable;

public class CreateTable(ITableWriter tableWriter, IUnitOfWork unitOfWork) : ICreateTable
{
    private readonly ITableWriter _tableWriter = tableWriter;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TableResponse> Execute(CreateTableRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var table = request.Adapt<Domain.Entities.Table>();

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