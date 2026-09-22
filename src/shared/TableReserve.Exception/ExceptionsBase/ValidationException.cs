using System.Net;

namespace TableReserve.Exception.ExceptionsBase;

public class ValidationException : TableReserveException
{
    private readonly List<string> _errors;
    public ValidationException(List<string> errorMessages) => _errors = errorMessages;
    public override List<string> GetErrorMessages() => _errors;

    public override int StatusCode() => (int)HttpStatusCode.BadRequest;
}