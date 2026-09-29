using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;

namespace TableReserve.Application.UseCases.Table.CreateTable;

public interface ICreateTable
{
    Task<TableResponse> Execute(CreateTableRequest request, CancellationToken cancellationToken);
}