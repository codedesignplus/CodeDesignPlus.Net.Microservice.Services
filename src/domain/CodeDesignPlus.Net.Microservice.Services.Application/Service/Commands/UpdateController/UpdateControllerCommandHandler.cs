namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateController;

public class UpdateControllerCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<UpdateControllerCommand>
{
    public async Task Handle(UpdateControllerCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindAsync<ServiceAggregate>(request.IdService, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        service.UpdateController(request.IdController,request.Name, request.Description, user.IdUser);

        await repository.UpdateAsync(service, cancellationToken);

        // El detalle se cachea en GetServiceById: sin esto se sirve la version anterior (pendings/018).
        await cacheManager.RemoveAsync(service.Id.ToString());

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }
}