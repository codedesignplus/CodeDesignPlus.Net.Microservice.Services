namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RemoveController;

public class RemoveControllerCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<RemoveControllerCommand>
{
    public async Task Handle(RemoveControllerCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindAsync<ServiceAggregate>(request.IdService, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        service.RemoveController(request.IdController, user.IdUser);

        await repository.UpdateAsync(service, cancellationToken);

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }
}