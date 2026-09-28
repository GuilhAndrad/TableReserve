using TableReserve.Domain.Enums;

namespace TableReserve.Domain.Entities;

public class Table : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public TableStatus Status { get; set; } = TableStatus.Available;
}
