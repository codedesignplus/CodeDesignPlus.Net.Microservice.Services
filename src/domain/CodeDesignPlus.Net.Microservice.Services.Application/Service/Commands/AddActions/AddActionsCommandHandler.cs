namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddActions;

public class AddActionsCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<AddActionsCommand>
{
    public async Task Handle(AddActionsCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindAsync<ServiceAggregate>(request.IdService, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        foreach (var action in request.Actions)
        {
            ApplicationGuard.IsNull(action, Errors.InvalidRequest);

            service.AddAction(request.IdController, request.Controller, action.Id, action.Name, action.Description, action.HttpMethod, user.IsAuthenticated ? user.IdUser : Guid.Empty);
        }

        await repository.UpdateAsync(service, cancellationToken);

        // El detalle se cachea en GetServiceById: sin esto se sirve la version anterior (pendings/018).
        await cacheManager.RemoveAsync(service.Id.ToString());

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }
}