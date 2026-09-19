using TableReserve.Domain.Repositories;

namespace TableReserve.Infrastructure.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly TableReserveDbContext _dbContext;
    public UnitOfWork(TableReserveDbContext dbContext) => _dbContext = dbContext;
    public async Task Commit(CancellationToken cancellationToken) => await _dbContext.SaveChangesAsync(cancellationToken);
}