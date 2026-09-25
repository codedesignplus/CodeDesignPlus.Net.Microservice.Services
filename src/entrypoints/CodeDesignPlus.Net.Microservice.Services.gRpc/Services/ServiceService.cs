using CodeDesignPlus.Net.Exceptions.Guards;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RegisterService;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceByName;
using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;
using Google.Protobuf.WellKnownTypes;

namespace CodeDesignPlus.Net.Microservice.Services.gRpc.Services;

public class ServiceService(IMediator mediator, IMapper mapper, ILogger<ServiceService> logger) : Service.ServiceBase
{
    /// <summary>
    /// Lo llama cada microservicio al arrancar (RegisterResourcesBackgroundService del SDK) con todos sus controllers y
    /// acciones. Se resuelve en un solo comando y una sola escritura (pendings/018).
    /// </summary>
    public override async Task<Empty> CreateService(CreateServiceRequest request, ServerCallContext context)
    {
        var controllers = request.Service.Controllers.Select(controller => new ControllerDto
        {
            Id = Guid.Parse(controller.Id),
            Name = controller.Name,
            Description = controller.Description,
            Actions = mapper.Map<List<ActionDto>>(controller.Actions)
        }).ToList();

        var command = new RegisterServiceCommand(Guid.Parse(request.Service.Id), request.Service.Name, request.Service.Description, controllers);

        await mediator.Send(command, context.CancellationToken);

        return new Empty();
    }

    public async override Task<GetServiceResponse> GetService(GetServiceRequest request, ServerCallContext context)
    {
        var query = new GetServiceByNameQuery(request.Name);

        var result = await mediator.Send(query);

        InfrastructureGuard.IsNull(result, Infrastructure.Errors.ResourceNotFound);

        var response = new GetServiceResponse
        {
            Service = mapper.Map<Microservice>(result)
        };

        return response;
    }
}

