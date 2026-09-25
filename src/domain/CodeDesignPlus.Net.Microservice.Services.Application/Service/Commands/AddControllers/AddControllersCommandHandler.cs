namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddControllers;

public class AddControllersCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<AddControllersCommand>
{
    public async Task Handle(AddControllersCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindAsync<ServiceAggregate>(request.IdService, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        foreach (var controller in request.Controllers)
        {
            ApplicationGuard.IsNull(controller, Errors.InvalidRequest);

            service.AddController(controller.Id, controller.Name, controller.Description, user.IsAuthenticated ? user.IdUser : Guid.Empty);
        }

        await repository.UpdateAsync(service, cancellationToken);

        // El detalle se cachea en GetServiceById: sin esto se sirve la version anterior (pendings/018).
        await cacheManager.RemoveAsync(service.Id.ToString());

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }
}