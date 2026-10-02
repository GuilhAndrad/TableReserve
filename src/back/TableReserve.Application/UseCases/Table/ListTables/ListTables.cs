using Mapster;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Repositories.Table;

namespace TableReserve.Application.UseCases.Table.ListTables;

public class ListTables(ITableReader tableReader) : IListTables
{
    private readonly ITableReader _tableReader = tableReader;

    public async Task<List<TableResponse>> Execute(CancellationToken cancellationToken)
    {
        var tables = await _tableReader.GetAll(cancellationToken);

        return [.. tables.Select(table => new TableResponse
        {
            Id = table.Id,
            Name = table.Name,
            Capacity = table.Capacity,
            Status = table.Status.Adapt<Communication.Enums.TableStatus>()
        })];
    }
}