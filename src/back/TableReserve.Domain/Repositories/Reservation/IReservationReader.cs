namespace TableReserve.Domain.Repositories.Reservation;

public interface IReservationReader
{
    Task<Entities.Reservation?> GetById(Guid id, CancellationToken cancellationToken);
    Task<List<Entities.Reservation>> GetAllByUser(Guid userId, CancellationToken cancellationToken);
    Task<bool> ExistOverlappingActiveReservation(Guid tableId, DateTime start, DateTime end, CancellationToken cancellationToken);
}
