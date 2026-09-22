using System.Net;

namespace TableReserve.Exception.ExceptionsBase;

public class InvalidLoginException : TableReserveException
{
    public override List<string> GetErrorMessages() => [MessagesExceptionResource.LOGIN_INVALID_VALIDATION];
    public override int StatusCode() => (int)HttpStatusCode.Unauthorized;
}