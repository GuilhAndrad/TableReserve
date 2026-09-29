using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;

namespace TableReserve.Application.UseCases.Table.UpdateTable;

public interface IUpdateTable
{
    Task<TableResponse> Execute(Guid id, UpdateTableRequest request, CancellationToken cancellationToken);
}