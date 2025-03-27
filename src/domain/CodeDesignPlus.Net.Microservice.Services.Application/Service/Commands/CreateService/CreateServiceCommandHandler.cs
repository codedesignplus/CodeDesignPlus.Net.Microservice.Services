namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.CreateService;

public class CreateServiceCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateServiceCommand>
{
    public async Task Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exist = await repository.ExistsAsync<ServiceAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.ServiceAlreadyExists);

        var service = ServiceAggregate.Create(request.Id, request.Name, request.Description, user.IsAuthenticated ? user.IdUser : Guid.Empty);

        await repository.CreateAsync(service, cancellationToken);

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }
}