namespace TableReserve.Application.UseCases.Reservation.CancelReservation;

public interface ICancelReservation
{
    Task Execute(Guid id, CancellationToken cancellationToken);
}