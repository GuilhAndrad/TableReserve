using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;

namespace TableReserve.Application.UseCases.Token.RefreshToken;

public interface IRefreshTokenUseCase
{
    Task<TokensResponse> Execute(RefreshTokenRequest request, CancellationToken cancellationToken);
}