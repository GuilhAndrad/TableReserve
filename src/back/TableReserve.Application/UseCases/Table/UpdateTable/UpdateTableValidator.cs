using FluentValidation;
using TableReserve.Communication.Enums;
using TableReserve.Communication.Requests;
using TableReserve.Exception;

namespace TableReserve.Application.UseCases.Table.UpdateTable;

public class UpdateTableValidator : AbstractValidator<UpdateTableRequest>
{
    public UpdateTableValidator()
    {
        RuleFor(table => table.Name)
            .NotEmpty()
            .WithMessage(MessagesExceptionResource.NAME_REQUIRED_VALIDATION);

        RuleFor(table => table.Capacity)
            .GreaterThan(0)
            .WithMessage(MessagesExceptionResource.TABLE_CAPACITY_MIN_VALIDATION);

        RuleFor(table => table.Status)
            .IsInEnum()
            .WithMessage(MessagesExceptionResource.TABLE_STATUS_INVALID_VALIDATION);
    }
}