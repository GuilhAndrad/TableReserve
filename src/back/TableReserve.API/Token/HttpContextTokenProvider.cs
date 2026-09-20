using TableReserve.Domain.Security.Tokens;

namespace TableReserve.API.Token;

internal sealed class HttpContextTokenProvider : IAccessTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public string GetToken()
    {
        string accessToken = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return accessToken["Bearer ".Length..];
    }
}