using TableReserve.Communication.Enums;

namespace TableReserve.Communication.Requests;

public class UpdateTableRequest
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public TableStatus Status { get; set; }
}