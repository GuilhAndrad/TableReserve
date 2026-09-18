namespace TableReserve.Domain.Security.Tokens;

public interface IAccessTokenProvider
{
    string GetToken();
}