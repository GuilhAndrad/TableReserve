namespace TableReserve.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(Entities.User user);
}