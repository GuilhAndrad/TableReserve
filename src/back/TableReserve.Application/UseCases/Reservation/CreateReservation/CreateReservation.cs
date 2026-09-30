using Mapster;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Enums;
using TableReserve.Domain.Repositories;
using TableReserve.Domain.Repositories.Reservation;
using TableReserve.Domain.Repositories.Table;
using TableReserve.Domain.Repositories.User;
using TableReserve.Exception;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.Application.UseCases.Reservation.CreateReservation;

public class CreateReservation : ICreateReservation
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITableReader _tableReader;
    private readonly IReservationReader _reservationReader;
    private readonly IReservationWriter _reservationWriter;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReservation(
        ILoggedUser loggedUser,
        ITableReader tableReader,
        IReservationReader reservationReader,
        IReservationWriter reservationWriter,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _tableReader = tableReader;
        _reservationReader = reservationReader;
        _reservationWriter = reservationWriter;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReservationResponse> Execute(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var table = await _tableReader.GetById(request.TableId, cancellationToken)
            ?? throw new NotFoundException(MessagesExceptionResource.TABLE_NOT_FOUND_VALIDATION);

        if (table.Status != TableStatus.Available)
            throw new ValidationException([MessagesExceptionResource.TABLE_UNAVAILABLE_VALIDATION]);

        if (request.GuestsCount > table.Capacity)
            throw new ValidationException([MessagesExceptionResource.TABLE_CAPACITY_EXCEEDED_VALIDATION]);

        var reservationEnd = request.ReservationDate.AddMinutes(request.DurationMinutes);

        var hasOverlap = await _reservationReader.ExistOverlappingActiveReservation(
            request.TableId, request.ReservationDate, reservationEnd, cancellationToken);

        if (hasOverlap)
            throw new ValidationException([MessagesExceptionResource.RESERVATION_TABLE_UNAVAILABLE_VALIDATION]);

        var reservation = new Domain.Entities.Reservation
        {
            UserId = _loggedUser.GetUserId(),
            TableId = request.TableId,
            ReservationDate = request.ReservationDate,
            DurationMinutes = request.DurationMinutes,
            GuestsCount = request.GuestsCount
        };

        await _reservationWriter.Add(reservation, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return new ReservationResponse
        {
            Id = reservation.Id,
            TableId = reservation.TableId,
            ReservationDate = reservation.ReservationDate,
            DurationMinutes = reservation.DurationMinutes,
            GuestsCount = reservation.GuestsCount,
            Status = reservation.Status.Adapt<Communication.Enums.ReservationStatus>()
        };
    }

    private static void Validate(CreateReservationRequest request)
    {
        var validator = new CreateReservationValidator();
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidationException(errorMessages);
        }
    }
}