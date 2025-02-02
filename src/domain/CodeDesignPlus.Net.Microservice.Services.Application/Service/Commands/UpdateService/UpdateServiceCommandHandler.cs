namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;

public class UpdateServiceCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateServiceCommand>
{
    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindAsync<ServiceAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        service.Update(request.Name, request.Description, request.IsActive, user.IdUser);

        await repository.UpdateAsync(service, cancellationToken);

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }
}