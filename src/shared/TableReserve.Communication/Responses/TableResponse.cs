using TableReserve.Communication.Enums;

namespace TableReserve.Communication.Responses;

public class TableResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public TableStatus Status { get; set; }
}