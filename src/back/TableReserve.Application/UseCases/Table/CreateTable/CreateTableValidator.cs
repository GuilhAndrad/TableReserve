using FluentValidation;
using TableReserve.Communication.Requests;
using TableReserve.Exception;

namespace TableReserve.Application.UseCases.Table.CreateTable;

public class CreateTableValidator : AbstractValidator<CreateTableRequest>
{
    public CreateTableValidator()
    {
        RuleFor(table => table.Name).NotEmpty().WithMessage(MessagesExceptionResource.NAME_REQUIRED_VALIDATION);
        RuleFor(table => table.Capacity).GreaterThan(0).WithMessage(MessagesExceptionResource.TABLE_CAPACITY_MIN_VALIDATION);
    }
}