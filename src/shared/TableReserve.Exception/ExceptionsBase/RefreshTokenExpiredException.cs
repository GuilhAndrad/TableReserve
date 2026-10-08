using System.Net;

namespace TableReserve.Exception.ExceptionsBase;

public class RefreshTokenExpiredException : TableReserveException
{
    public override List<string> GetErrorMessages() => [MessagesExceptionResource.REFRESH_TOKEN_EXPIRED];

    public override int StatusCode() => (int)HttpStatusCode.Unauthorized;
}
