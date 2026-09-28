using Microsoft.EntityFrameworkCore;
using TableReserve.Domain.Repositories.Table;

namespace TableReserve.Infrastructure.DataAccess.Repositories;

internal sealed class TableRepository(TableReserveDbContext dbContext) : ITableWriter, ITableReader
{
    private readonly TableReserveDbContext _dbContext = dbContext;

    public async Task Add(Domain.Entities.Table table, CancellationToken cancellationToken) =>
        await _dbContext.Tables.AddAsync(table, cancellationToken);

    public async Task<List<Domain.Entities.Table>> GetAll(CancellationToken cancellationToken)
    {
        return await _dbContext.Tables.AsNoTracking().Where(table => table.Active).ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entities.Table?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Tables.SingleOrDefaultAsync(table => table.Active && table.Id == id, cancellationToken);
    }
}