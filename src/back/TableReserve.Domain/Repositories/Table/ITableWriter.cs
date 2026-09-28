namespace TableReserve.Domain.Repositories.Table;

public interface ITableWriter
{
    Task Add(Entities.Table table, CancellationToken cancellationToken);
}