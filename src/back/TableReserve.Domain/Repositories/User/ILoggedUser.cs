namespace TableReserve.Domain.Repositories.User;

public interface ILoggedUser
{
    Task<Entities.User> Get(CancellationToken cancellationToken);
    Guid GetUserId();
}