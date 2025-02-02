namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateAction;

public class UpdateActionCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateActionCommand>
{
    public async Task Handle(UpdateActionCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindAsync<ServiceAggregate>(request.IdService, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        service.UpdateAction(request.IdController, request.IdAction, request.Name, request.Description, request.HttpMethod, user.IdUser);

        await repository.UpdateAsync(service, cancellationToken);

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }
}