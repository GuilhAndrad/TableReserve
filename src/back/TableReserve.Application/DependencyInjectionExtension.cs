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
        services.AddScoped<UseCases.Table.CreateTable.ICreateTable, UseCases.Table.CreateTable.CreateTable>();
        services.AddScoped<UseCases.Table.ListTables.IListTables, UseCases.Table.ListTables.ListTables>();
        services.AddScoped<UseCases.Table.UpdateTable.IUpdateTable, UseCases.Table.UpdateTable.UpdateTable>();
        services.AddScoped<UseCases.Reservation.CreateReservation.ICreateReservation, UseCases.Reservation.CreateReservation.CreateReservation>();
        services.AddScoped<UseCases.Reservation.ListReservations.IListReservations, UseCases.Reservation.ListReservations.ListReservations>();
        services.AddScoped<UseCases.Reservation.CancelReservation.ICancelReservation, UseCases.Reservation.CancelReservation.CancelReservation>();
    }
}
