using TableReserve.Communication.Requests;
using TableReserve.Communication.Responses;

namespace TableReserve.Application.UseCases.User.Login;

public interface ILoginUser
{
    Task<RegisteredUserResponse> Execute(LoginUserRequest request, CancellationToken cancellationToken);
}
