using FluentValidation;
using TableReserve.Communication.Requests;
using TableReserve.Exception;

namespace TableReserve.Application.UseCases.Reservation.CreateReservation;

public class CreateReservationValidator : AbstractValidator<CreateReservationRequest>
{
    private const int MinDurationMinutes = 20;
    private const int MaxDurationMinutes = 120;
    private const int DurationStepMinutes = 20;

    private static readonly TimeSpan OpeningTime = new(8, 0, 0);
    private static readonly TimeSpan ClosingTime = new(23, 0, 0);

    public CreateReservationValidator()
    {
        RuleFor(reservation => reservation.ReservationDate)
            .NotEqual(default(DateTime)).WithMessage(MessagesExceptionResource.RESERVATION_DATE_REQUIRED_VALIDATION);

        RuleFor(reservation => reservation.GuestsCount)
            .GreaterThan(0).WithMessage(MessagesExceptionResource.RESERVATION_GUESTS_REQUIRED_VALIDATION);

        RuleFor(reservation => reservation.DurationMinutes)
            .Must(duration =>
                duration >= MinDurationMinutes &&
                duration <= MaxDurationMinutes &&
                duration % DurationStepMinutes == 0)
            .WithMessage(MessagesExceptionResource.RESERVATION_DURATION_INVALID_VALIDATION);

        When(reservation => reservation.ReservationDate != default, () =>
        {
            RuleFor(reservation => reservation.ReservationDate)
                .GreaterThan(DateTime.Now).WithMessage(MessagesExceptionResource.RESERVATION_DATE_FUTURE_VALIDATION);

            RuleFor(reservation => reservation.ReservationDate.TimeOfDay)
                .Must(time => time >= OpeningTime && time <= ClosingTime)
                .WithMessage(MessagesExceptionResource.RESERVATION_OUTSIDE_BUSINESS_HOURS_VALIDATION);

            When(reservation => reservation.DurationMinutes > 0, () =>
            {
                RuleFor(reservation => reservation)
                    .Must(reservation =>
                        reservation.ReservationDate.AddMinutes(reservation.DurationMinutes)
                            <= reservation.ReservationDate.Date.Add(ClosingTime))
                    .WithMessage(MessagesExceptionResource.RESERVATION_OUTSIDE_BUSINESS_HOURS_VALIDATION);
            });
        });
    }
}