namespace TableReserve.Domain.Repositories.Reservation;

public interface IReservationWriter
{
    Task Add(Entities.Reservation reservation, CancellationToken cancellationToken);
}