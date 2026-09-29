using TableReserve.Communication.Responses;

namespace TableReserve.Application.UseCases.Table.ListTables;

public interface IListTables
{
    Task<List<TableResponse>> Execute(CancellationToken cancellationToken);
}