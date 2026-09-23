using TableReserve.Communication.Responses;

namespace TableReserve.Application.UseCases.User.GetUser;

public interface IGetUser
{
    Task<UserResponse> Execute(CancellationToken cancellationToken);
}
