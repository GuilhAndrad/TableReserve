using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;

namespace TableReserve.Application.UseCases.Reservation.CreateReservation;

public interface ICreateReservation
{
    Task<ReservationResponse> Execute(CreateReservationRequest request, CancellationToken cancellationToken);
}
