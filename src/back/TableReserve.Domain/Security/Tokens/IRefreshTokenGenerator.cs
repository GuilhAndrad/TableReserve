namespace TableReserve.Domain.Security.Tokens;

public interface IRefreshTokenGenerator
{
    string Generate();
}