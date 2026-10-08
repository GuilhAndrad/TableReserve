namespace TableReserve.Domain.Repositories.RefreshToken;

public interface IRefreshTokenWrite
{
    Task Replace(Entities.RefreshToken refreshToken);
}
