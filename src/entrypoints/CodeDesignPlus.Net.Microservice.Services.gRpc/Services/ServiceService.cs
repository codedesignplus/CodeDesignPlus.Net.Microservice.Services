using CodeDesignPlus.Net.Exceptions.Guards;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddAction;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddActions;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddController;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddControllers;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.CreateService;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceById;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceByName;
using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;
using Google.Protobuf.WellKnownTypes;

namespace CodeDesignPlus.Net.Microservice.Services.gRpc.Services;

public class ServiceService(IMediator mediator, IMapper mapper, ILogger<ServiceService> logger) : Service.ServiceBase
{
    public override async Task<Empty> CreateService(CreateServiceRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.Service.Id);

        await CreateServiceAsync(request, id);

        var controllerCommand = new AddControllersCommand(id, mapper.Map<List<ControllerDto>>(request.Service.Controllers));
        await mediator.Send(controllerCommand);

        foreach (var controller in request.Service.Controllers)
        {
            var controllerId = Guid.Parse(controller.Id);

            var actionCommand = new AddActionsCommand(id, controllerId, controller.Name, mapper.Map<List<ActionDto>>(controller.Actions));

            await mediator.Send(actionCommand);
        }

        return new Empty();
    }

    private async Task CreateServiceAsync(CreateServiceRequest request, Guid id)
    {
        ServiceDto service = null!;

        try
        {
            var query = new GetServiceByIdQuery(id);
            service = await mediator.Send(query);
        }
        catch (CodeDesignPlusException ex)
        {
            logger.LogWarning(ex, ex.Message);
        }
        finally
        {
            if (service == null)
            {
                var createCommand = new CreateServiceCommand(id, request.Service.Name, request.Service.Description);
                await mediator.Send(createCommand);
            }
        }
    }

    public async override Task<GetServiceResponse> GetService(GetServiceRequest request, ServerCallContext context)
    {
        var query = new GetServiceByNameQuery(request.Name);

        var result = await mediator.Send(query);

        InfrastructureGuard.IsNull(result, "301 : The resource was not found");

        var response = new GetServiceResponse
        {
            Service = mapper.Map<Microservice>(result)
        };

        return response;
    }
}

