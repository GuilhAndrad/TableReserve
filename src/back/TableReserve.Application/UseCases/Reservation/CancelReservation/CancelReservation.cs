using TableReserve.Domain.Enums;
using TableReserve.Domain.Repositories;
using TableReserve.Domain.Repositories.Reservation;
using TableReserve.Domain.Repositories.User;
using TableReserve.Exception;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.Application.UseCases.Reservation.CancelReservation;

public class CancelReservation : ICancelReservation
{
    private readonly ILoggedUser _loggedUser;
    private readonly IReservationReader _reservationReader;
    private readonly IUnitOfWork _unitOfWork;

    public CancelReservation(ILoggedUser loggedUser, IReservationReader reservationReader, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _reservationReader = reservationReader;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id, CancellationToken cancellationToken)
    {
        var reservation = await _reservationReader.GetById(id, cancellationToken)
            ?? throw new NotFoundException(MessagesExceptionResource.RESERVATION_NOT_FOUND_VALIDATION);

        if (reservation.UserId != _loggedUser.GetUserId())
            throw new ForbiddenException();

        if (reservation.Status == ReservationStatus.Cancelled)
            throw new ValidationException([MessagesExceptionResource.RESERVATION_ALREADY_CANCELLED_VALIDATION]);

        reservation.Status = ReservationStatus.Cancelled;

        await _unitOfWork.Commit(cancellationToken);
    }
}