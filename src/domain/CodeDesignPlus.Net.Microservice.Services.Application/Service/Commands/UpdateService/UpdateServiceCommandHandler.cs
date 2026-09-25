namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;

public class UpdateServiceCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<UpdateServiceCommand>
{
    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindAsync<ServiceAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        service.Update(request.Name, request.Description, request.IsActive, user.IdUser);

        await repository.UpdateAsync(service, cancellationToken);

        // El detalle se cachea en GetServiceById: sin esto se sirve la version anterior (pendings/018).
        await cacheManager.RemoveAsync(service.Id.ToString());

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }
}