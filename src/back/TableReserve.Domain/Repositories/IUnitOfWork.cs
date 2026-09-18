namespace TableReserve.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
}