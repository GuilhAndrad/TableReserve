namespace TableReserve.Exception.ExceptionsBase;

public abstract class TableReserveException : System.Exception
{
    public abstract int StatusCode();
    public abstract List<string> GetErrorMessages();
}