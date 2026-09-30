using Mapster;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Repositories.Reservation;
using TableReserve.Domain.Repositories.User;

namespace TableReserve.Application.UseCases.Reservation.ListReservations;

public class ListReservations : IListReservations
{
    private readonly ILoggedUser _loggedUser;
    private readonly IReservationReader _reservationReader;

    public ListReservations(ILoggedUser loggedUser, IReservationReader reservationReader)
    {
        _loggedUser = loggedUser;
        _reservationReader = reservationReader;
    }

    public async Task<List<ReservationResponse>> Execute(CancellationToken cancellationToken)
    {
        var reservations = await _reservationReader.GetAllByUser(_loggedUser.GetUserId(), cancellationToken);

        return reservations.Select(reservation => new ReservationResponse
        {
            Id = reservation.Id,
            TableId = reservation.TableId,
            ReservationDate = reservation.ReservationDate,
            DurationMinutes = reservation.DurationMinutes,
            GuestsCount = reservation.GuestsCount,
            Status = reservation.Status.Adapt<Communication.Enums.ReservationStatus>()
        }).ToList();
    }
}