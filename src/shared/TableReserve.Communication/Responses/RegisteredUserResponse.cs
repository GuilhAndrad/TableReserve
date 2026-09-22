namespace TableReserve.Communication.Responses;

public class RegisteredUserResponse
{
    public string Name { get; set; } = string.Empty;
    public TokensResponse Tokens { get; set; } = new TokensResponse();
}