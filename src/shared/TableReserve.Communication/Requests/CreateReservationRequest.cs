namespace TableReserve.Communication.Requests;

public class CreateReservationRequest
{
    public Guid TableId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int GuestsCount { get; set; }
}