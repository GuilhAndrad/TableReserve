using FluentValidation.Results;
using Mapster;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Entities;
using TableReserve.Domain.Repositories;
using TableReserve.Domain.Repositories.RefreshToken;
using TableReserve.Domain.Repositories.User;
using TableReserve.Domain.Security.PasswordHashing;
using TableReserve.Domain.Security.Tokens;
using TableReserve.Exception;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.Application.UseCases.User.RegisterAccount;

public class RegisterUser(
    IPasswordHasher passwordHasher,
    IUserWriter userWriter,
    IUserReader userReader,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenWrite refreshTokenWrite,
    IUnitOfWork unitOfWork) : IRegisterUser
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserWriter _userWriter = userWriter;
    private readonly IUserReader _userReader = userReader;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
    private readonly IRefreshTokenWrite _refreshTokenWriter = refreshTokenWrite;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RegisteredUserResponse> Execute(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        await Validate(request, cancellationToken);

        var user = request.Adapt<Domain.Entities.User>();

        user.Password = _passwordHasher.HashPassword(request.Password);

        await _userWriter.Add(user, cancellationToken);

        var refreshToken = await GenerateRefreshToken(user.Id);

        await _unitOfWork.Commit(cancellationToken);

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

    private async Task Validate(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        var emailExist = await _userReader.ExistActiveUserWithEmail(request.Email, cancellationToken);
        if (emailExist)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, MessagesExceptionResource.EMAIL_INVALID_VALIDATION));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ValidationException(errorMessages);
        }
    }
}
