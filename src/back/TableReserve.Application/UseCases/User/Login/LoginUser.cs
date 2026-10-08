using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Repositories.RefreshToken;
using TableReserve.Domain.Repositories.User;
using TableReserve.Domain.Security.PasswordHashing;
using TableReserve.Domain.Security.Tokens;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.Application.UseCases.User.Login;

public class LoginUser(
    IPasswordHasher passwordHasher, 
    IUserReader userReader, 
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenWrite refreshTokenWriter,
    IRefreshTokenGenerator refreshTokenGenerator) : ILoginUser
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserReader _userReader = userReader;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;

    private readonly IRefreshTokenWrite _refreshTokenWriter = refreshTokenWriter;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;

    public async Task<RegisteredUserResponse> Execute(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userReader.GetByEmail(request.Email, cancellationToken) ?? throw new InvalidLoginException();

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);

        if (!isPasswordValid)
            throw new InvalidLoginException();

        var refreshToken = await GenerateRefreshToken(user.Id);

        return new RegisteredUserResponse
        {
            Name = user.Name,
            Tokens = new TokensResponse
            {
                AccessToken = _accessTokenGenerator.Generate(user),
                RefreshToken = refreshToken
            }
        };
    }

    private async Task<string> GenerateRefreshToken(Guid userId)
    {

        var refreshToken = new Domain.Entities.RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow

        };

        await _refreshTokenWriter.Replace(refreshToken);

        return refreshToken.Value;
    }
}