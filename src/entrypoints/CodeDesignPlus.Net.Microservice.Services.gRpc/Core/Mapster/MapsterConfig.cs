using CodeDesignPlus.Net.Microservice.Services.Domain;
using CodeDesignPlus.Net.Microservice.Services.Domain.Entities;
using NodaTime;
using NodaTime.Serialization.Protobuf;

namespace CodeDesignPlus.Net.Microservice.Services.gRpc.Core.Mapster;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<ServiceDto, Microservice>
            .NewConfig()
            .AfterMapping((src, dest) =>
            {
                dest.Controllers.AddRange(src.Controllers.Select(c => c.Adapt<Controller>()));
            });

        TypeAdapterConfig<ControllerDto, Controller>
            .NewConfig()
            .AfterMapping((src, dest) =>
            {
                dest.Actions.AddRange(src.Actions.Select(a => a.Adapt<Action>()));
            });

        TypeAdapterConfig<ActionDto, Action>.NewConfig();
    }
}