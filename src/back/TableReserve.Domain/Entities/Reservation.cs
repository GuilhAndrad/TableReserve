using TableReserve.Domain.Enums;

namespace TableReserve.Domain.Entities;

public class Reservation
{
    public Guid UserId { get; set; }
    public Guid TableId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int DurationMinutes { get; set; }
    public int GuestsCount { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Active;
}
