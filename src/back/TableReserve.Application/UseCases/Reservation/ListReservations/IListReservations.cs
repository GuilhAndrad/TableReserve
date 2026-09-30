using TableReserve.Communication.Responses;

namespace TableReserve.Application.UseCases.Reservation.ListReservations;

public interface IListReservations
{
    Task<List<ReservationResponse>> Execute(CancellationToken cancellationToken);
}
