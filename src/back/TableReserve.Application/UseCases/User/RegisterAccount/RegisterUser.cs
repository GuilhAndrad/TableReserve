using FluentValidation.Results;
using Mapster;
using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Repositories;
using TableReserve.Domain.Repositories.User;
using TableReserve.Domain.Security.PasswordHashing;
using TableReserve.Domain.Security.Tokens;
using TableReserve.Exception;
using TableReserve.Exception.ExceptionsBase;

namespace TableReserve.Application.UseCases.User.RegisterAccount;

public class RegisterUser : IRegisterUser
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserWriter _userWriter;
    private readonly IUserReader _userReader;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    private readonly IUnitOfWork _unitOfWork;

    public RegisterUser(
        IPasswordHasher passwordHasher,
        IUserWriter userWriter,
        IUserReader userReader,
        IAccessTokenGenerator accessTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _userWriter = userWriter;
        _userReader = userReader;
        _accessTokenGenerator = accessTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisteredUserResponse> Execute(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        await ValidateAndThrowOnFailures(request, cancellationToken);

        var user = request.Adapt<Domain.Entities.User>();

        user.Password = _passwordHasher.HashPassword(request.Password);

        await _userWriter.Add(user, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return new RegisteredUserResponse
        {
            Name = user.Name,
            Tokens = new TokensResponse
            {
                AccessToken = _accessTokenGenerator.Generate(user)
            }
        };
    }

    private async Task ValidateAndThrowOnFailures(RegisterUserRequest request, CancellationToken cancellationToken)
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
