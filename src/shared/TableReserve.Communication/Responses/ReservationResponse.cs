using TableReserve.Communication.Enums;

namespace TableReserve.Communication.Responses;

public class ReservationResponse
{
    public Guid Id { get; set; }
    public Guid TableId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int DurationMinutes { get; set; }
    public int GuestsCount { get; set; }
    public ReservationStatus Status { get; set; }
}