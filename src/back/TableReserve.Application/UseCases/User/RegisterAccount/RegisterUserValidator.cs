using FluentValidation;
using TableReserve.Communication.Requests;
using TableReserve.Exception;

namespace TableReserve.Application.UseCases.User.RegisterAccount;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(MessagesExceptionResource.NAME_REQUIRED_VALIDATION);
        RuleFor(user => user.Email).NotEmpty().WithMessage(MessagesExceptionResource.EMAIL_REQUIRED_VALIDATION);
        RuleFor(user => user.Password).NotEmpty().WithMessage(MessagesExceptionResource.PASSWORD_REQUIRED_VALIDATION);

        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email)
                .EmailAddress().WithMessage(MessagesExceptionResource.EMAIL_INVALID_VALIDATION);
        });
        When(user => !string.IsNullOrEmpty(user.Password), () =>
        {
            RuleFor(user => user.Password)
                .MinimumLength(8).WithMessage(MessagesExceptionResource.PASSWORD_MIN_LENGTH_VALIDATION);
        });
        When(user => !string.IsNullOrEmpty(user.Name), () =>
        {
            RuleFor(user => user.Name)
                .MinimumLength(2).WithMessage(MessagesExceptionResource.NAME_MIN_LENGTH_VALIDATION);
        });
        When(user => !string.IsNullOrEmpty(user.Password), () =>
        {
            RuleFor(user => user.Password)
                .Matches(@"[A-Z]+").WithMessage(MessagesExceptionResource.PASSWORD_UPPERCASE_VALIDATION)
                .Matches(@"[a-z]+").WithMessage(MessagesExceptionResource.PASSWORD_LOWERCASE_VALIDATION)
                .Matches(@"[0-9]+").WithMessage(MessagesExceptionResource.PASSWORD_NUMBER_VALIDATION)
                .Matches(@"[\W_]+").WithMessage(MessagesExceptionResource.PASSWORD_SPECIAL_CHAR_VALIDATION);
        });
    }
}