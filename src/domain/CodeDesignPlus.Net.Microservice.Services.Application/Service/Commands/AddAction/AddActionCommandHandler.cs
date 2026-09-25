namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddAction;

public class AddActionCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<AddActionCommand>
{
    public async Task Handle(AddActionCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindAsync<ServiceAggregate>(request.IdService, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        service.AddAction(request.IdController, request.IdAction, request.Name, request.Description, request.HttpMethod, user.IdUser);

        await repository.UpdateAsync(service, cancellationToken);

        // El detalle se cachea en GetServiceById: sin esto se sirve la version anterior (pendings/018).
        await cacheManager.RemoveAsync(service.Id.ToString());

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }
}