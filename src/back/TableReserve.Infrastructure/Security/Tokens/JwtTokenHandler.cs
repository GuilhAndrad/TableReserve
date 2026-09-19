using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TableReserve.Domain.Entities;
using TableReserve.Domain.Security.Tokens;

namespace TableReserve.Infrastructure.Security.Tokens;

internal sealed class JwtTokenHandler(uint tokenExpirationInMinutes, string secretKey) : IAccessTokenGenerator
{
    private readonly uint _tokenExpirationInMinutes = tokenExpirationInMinutes;
    private readonly string _secretKey = secretKey;

    public string Generate(User user)
    {
        List<Claim> claims =
        [
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationInMinutes),
            SigningCredentials = new SigningCredentials(Credentials(), SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };
        JsonWebTokenHandler handler = new();

        return handler.CreateToken(tokenDescriptor);
    }

    private SymmetricSecurityKey Credentials()
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(_secretKey);

        return new SymmetricSecurityKey(keyBytes);
    }
}