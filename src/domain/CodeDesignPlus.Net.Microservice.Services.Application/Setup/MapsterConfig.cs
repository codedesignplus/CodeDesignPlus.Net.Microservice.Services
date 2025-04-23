using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.CreateService;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;
using CodeDesignPlus.Net.Microservice.Services.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Setup;

public static class MapsterConfigService
{
    public static void Configure()
    {        
        //Services
        TypeAdapterConfig<CreateServiceDto, CreateServiceCommand>.NewConfig();
        TypeAdapterConfig<UpdateServiceDto, UpdateServiceCommand>.NewConfig();
        TypeAdapterConfig<ServiceAggregate, ServiceDto>.NewConfig();
        //TypeAdapterConfig<Pagination<ServiceAggregate>, Pagination<ServiceDto>>.NewConfig();


        //Controllers
        TypeAdapterConfig<ControllerEntity, ControllerDto>.NewConfig();

        //Actions
        TypeAdapterConfig<ActionEntity, ActionDto>.NewConfig();
    }
}
