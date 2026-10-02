using TableReserve.Domain.Security.Tokens;

namespace TableReserve.API.Token;

internal sealed class HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor) : IAccessTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetToken()
    {
        string accessToken = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return accessToken["Bearer ".Length..];
    }
}