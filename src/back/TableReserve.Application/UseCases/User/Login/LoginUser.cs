using System.Diagnostics.CodeAnalysis;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Repositories.User;
using TableReserve.Domain.Security.PasswordHashing;
using TableReserve.Domain.Security.Tokens;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.Application.UseCases.User.Login;

public class LoginUser : ILoginUser
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserReader _userReader;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginUser(IPasswordHasher passwordHasher, IUserReader userReader, IAccessTokenGenerator accessTokenGenerator)
    {
        _passwordHasher = passwordHasher;
        _userReader = userReader;
        _accessTokenGenerator = accessTokenGenerator;
    }
    public async Task<RegisteredUserResponse> Execute(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userReader.GetByEmail(request.Email, cancellationToken) ?? throw new InvalidLoginException();
        
        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);

        if(!isPasswordValid)
            throw new InvalidLoginException();

        return new RegisteredUserResponse
        {
            Name = user.Name,
            Tokens = new TokensResponse
            {
                AccessToken = _accessTokenGenerator.Generate(user),
            }
        };
    }
}