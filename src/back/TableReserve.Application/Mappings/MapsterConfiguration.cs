using Mapster;
using TableReserve.Communication.Requests;
using TableReserve.Domain.Entities;

namespace TableReserve.Application.Mappings;

internal static class MapsterConfiguration
{
    internal static void Configure()
    {
        TypeAdapterConfig<RegisterUserRequest, User>
            .NewConfig()
            .Ignore(dest => dest.Password);

        TypeAdapterConfig<CreateTableRequest, Table>.NewConfig();
        TypeAdapterConfig<UpdateTableRequest, Table>.NewConfig();
        TypeAdapterConfig<CreateReservationRequest, Reservation>.NewConfig();

        TypeAdapterConfig<Domain.Enums.TableStatus, Communication.Enums.TableStatus>.NewConfig();
        TypeAdapterConfig<Communication.Enums.TableStatus, Domain.Enums.TableStatus>.NewConfig();
        TypeAdapterConfig<Domain.Enums.ReservationStatus, Communication.Enums.ReservationStatus>.NewConfig();
    }
}
