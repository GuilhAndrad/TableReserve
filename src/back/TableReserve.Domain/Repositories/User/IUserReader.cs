namespace TableReserve.Domain.Repositories.User;

public interface IUserReader
{
    Task<bool> ExistActiveUserWithId(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistActiveUserWithEmail(string email, CancellationToken cancellationToken);
    Task<Entities.User?> GetByEmail(string email, CancellationToken cancellationToken);
    Task<Entities.User?> GetById(Guid userId, CancellationToken cancellationToken);
}