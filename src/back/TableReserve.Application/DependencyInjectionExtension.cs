using Microsoft.Extensions.DependencyInjection;
using TableReserve.Application.Mappings;

namespace TableReserve.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        MapsterConfiguration.Configure();

        services.AddUseCases();
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<UseCases.User.RegisterAccount.IRegisterUser, UseCases.User.RegisterAccount.RegisterUser>();
        services.AddScoped<UseCases.User.Login.ILoginUser, UseCases.User.Login.LoginUser>();
        services.AddScoped<UseCases.User.GetUser.IGetUser, UseCases.User.GetUser.GetUser>();
    }
}
