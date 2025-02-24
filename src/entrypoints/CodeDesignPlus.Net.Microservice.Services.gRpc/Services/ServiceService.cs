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

public class ServiceService(IMediator mediator, IMapper mapper) : Service.ServiceBase
{
    public override async Task<Empty> CreateService(CreateServiceRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.Service.Id);

        var createCommand = new CreateServiceCommand(id, request.Service.Name, request.Service.Description);
        await mediator.Send(createCommand);

        var controllerCommand = new AddControllersCommand(id, mapper.Map<List<ControllerDto>>(request.Service.Controllers));
        await mediator.Send(controllerCommand);        

        foreach (var controller in request.Service.Controllers)
        {
            var controllerId = Guid.Parse(controller.Id);

            var actionCommand = new AddActionsCommand(id, controllerId, mapper.Map<List<ActionDto>>(controller.Actions));

            await mediator.Send(actionCommand);
        }

        return new Empty();
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

