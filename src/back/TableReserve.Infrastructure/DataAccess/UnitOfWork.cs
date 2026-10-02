using TableReserve.Domain.Repositories;

namespace TableReserve.Infrastructure.DataAccess;

internal class UnitOfWork(TableReserveDbContext dbContext) : IUnitOfWork
{
    private readonly TableReserveDbContext _dbContext = dbContext;

    public async Task Commit(CancellationToken cancellationToken) => await _dbContext.SaveChangesAsync(cancellationToken);
}