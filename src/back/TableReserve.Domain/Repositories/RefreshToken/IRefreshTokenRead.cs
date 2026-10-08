namespace TableReserve.Domain.Repositories.RefreshToken;

public interface IRefreshTokenRead
{
    Task<Entities.RefreshToken?> Get(string refreshToken);
}
