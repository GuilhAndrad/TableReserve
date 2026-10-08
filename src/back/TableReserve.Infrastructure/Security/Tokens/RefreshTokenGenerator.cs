using System.Security.Cryptography;
using TableReserve.Domain.Security.Tokens;

namespace TableReserve.Infrastructure.Security.Tokens;

internal sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
    {
        var token = RandomNumberGenerator.GetBytes(32);

        return Convert.ToBase64String(token);
    }
}
