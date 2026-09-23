using TableReserve.Communication.Responses;
using TableReserve.Domain.Repositories.User;

namespace TableReserve.Application.UseCases.User.GetUser;

public class GetUser : IGetUser
{
    private readonly ILoggedUser _loggedUser;
    public GetUser(ILoggedUser loggedUser) => _loggedUser = loggedUser;
    public async Task<UserResponse> Execute(CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.Get(cancellationToken);

        return new UserResponse
        {
            Name = loggedUser.Name,
            Email = loggedUser.Email
        };
    }
}