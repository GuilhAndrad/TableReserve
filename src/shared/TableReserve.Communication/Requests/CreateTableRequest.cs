namespace TableReserve.Communication.Requests;

public class CreateTableRequest
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
}