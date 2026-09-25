namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.DeleteService;

public class DeleteServiceCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<DeleteServiceCommand>
{
    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<ServiceAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.ServiceNotFound);

        aggregate.Delete(user.IdUser);
        
        await repository.DeleteAsync<ServiceAggregate>(aggregate.Id, cancellationToken);

        // El detalle se cachea en GetServiceById: sin esto se sirve la version anterior (pendings/018).
        await cacheManager.RemoveAsync(aggregate.Id.ToString());

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}