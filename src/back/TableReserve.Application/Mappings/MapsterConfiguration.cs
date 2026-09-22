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
    }
}
