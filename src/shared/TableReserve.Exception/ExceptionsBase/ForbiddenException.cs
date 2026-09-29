using System.Net;

namespace TableReserve.Exception.ExceptionsBase;

public class ForbiddenException : TableReserveException
{
    public override List<string> GetErrorMessages() => [MessagesExceptionResource.ACCESS_DENIED_VALIDATION];
    public override int StatusCode() => (int)HttpStatusCode.Forbidden;
}