using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;

namespace TableReserve.Application.UseCases.User.RegisterAccount;

public interface IRegisterUser
{
    Task<RegisteredUserResponse> Execute(RegisterUserRequest request, CancellationToken cancellationToken);
}