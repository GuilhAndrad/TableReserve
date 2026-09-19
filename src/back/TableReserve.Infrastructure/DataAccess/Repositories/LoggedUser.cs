using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using TableReserve.Domain.Entities;
using TableReserve.Domain.Repositories.User;
using TableReserve.Domain.Security.Tokens;

namespace TableReserve.Infrastructure.DataAccess.Repositories;

internal class LoggedUser(IAccessTokenProvider accessTokenProvider, TableReserveDbContext dbContext) : ILoggedUser
{
    private readonly IAccessTokenProvider _accessTokenProvider = accessTokenProvider;
    private readonly TableReserveDbContext _dbContext = dbContext;

    public async Task<User> Get(CancellationToken cancellationToken)
    {
        Guid userId = GetUserId();

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.Id == userId, cancellationToken);
    }

    public Guid GetUserId()
    {
        string accessToken = _accessTokenProvider.GetToken();

        JsonWebTokenHandler handler = new();

        JsonWebToken jsonWebToken = handler.ReadJsonWebToken(accessToken);

        string subject = jsonWebToken.Subject;

        return Guid.Parse(subject);
    }
}
