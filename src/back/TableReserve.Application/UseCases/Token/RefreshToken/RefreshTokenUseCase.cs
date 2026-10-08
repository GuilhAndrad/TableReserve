using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Repositories;
using TableReserve.Domain.Repositories.RefreshToken;
using TableReserve.Domain.Repositories.User;
using TableReserve.Domain.Security.Tokens;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.Application.UseCases.Token.RefreshToken;

public class RefreshTokenUseCase(
    IRefreshTokenRead refreshTokenReader,
    IRefreshTokenWrite refreshTokenWriter,
    IUserReader userReader,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    IUnitOfWork unitOfWork) : IRefreshTokenUseCase
{
    private readonly IRefreshTokenRead _refreshTokenReader = refreshTokenReader;
    private readonly IRefreshTokenWrite _refreshTokenWriter = refreshTokenWriter;
    private readonly IUserReader _userReader = userReader;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private const int RefreshTokenExpirationDays = 7;

    public async Task<TokensResponse> Execute(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenReader.Get(request.RefreshToken) ?? throw new RefreshTokenExpiredException();

        var isValid = refreshToken.CreatedAt.AddDays(RefreshTokenExpirationDays) >= DateTime.UtcNow;
        if (!isValid)
        {
            throw new RefreshTokenExpiredException();
        }

        var user = await _userReader.GetById(refreshToken.UserId, cancellationToken);

        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = refreshToken.UserId,
            CreatedAt = DateTime.UtcNow

        };

        await _refreshTokenWriter.Replace(newRefreshToken);

        await _unitOfWork.Commit(cancellationToken);

        return new TokensResponse
        {
            AccessToken = _accessTokenGenerator.Generate(user!),
            RefreshToken = newRefreshToken.Value
        };
    }
}