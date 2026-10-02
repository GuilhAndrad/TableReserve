using System.Net;

namespace TableReserve.Exception.ExceptionsBase;

public class ValidationException(List<string> errorMessages) : TableReserveException
{
    private readonly List<string> _errors = errorMessages;

    public override List<string> GetErrorMessages() => _errors;

    public override int StatusCode() => (int)HttpStatusCode.BadRequest;
}