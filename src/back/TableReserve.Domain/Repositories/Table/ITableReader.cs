namespace TableReserve.Domain.Repositories.Table;

public interface ITableReader
{
    Task<List<Entities.Table>> GetAll(CancellationToken cancellationToken);
    Task<Entities.Table?> GetById(Guid id, CancellationToken cancellationToken);
}