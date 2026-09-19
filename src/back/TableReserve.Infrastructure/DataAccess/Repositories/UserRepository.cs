using Microsoft.EntityFrameworkCore;
using TableReserve.Domain.Entities;
using TableReserve.Domain.Repositories.User;

namespace TableReserve.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository(TableReserveDbContext dbContext) : IUserWriter, IUserReader
{
    private readonly TableReserveDbContext _dbContext = dbContext;

    public async Task Add(User user, CancellationToken cancellationToken) => await _dbContext.Users.AddAsync(user, cancellationToken);

    public async Task<bool> ExistActiveUserWithEmail(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.AnyAsync(user => user.Active && user.Email.Equals(email), cancellationToken);
    }

    public Task<bool> ExistActiveUserWithId(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Users.AnyAsync(user => user.Active && user.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Active && user.Email.Equals(email), cancellationToken);
    }

    public async Task<User?> GetById(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Active && user.Id == userId, cancellationToken);
    }
}