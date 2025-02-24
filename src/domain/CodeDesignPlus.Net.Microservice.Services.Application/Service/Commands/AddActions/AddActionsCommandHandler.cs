namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddActions;

public class AddActionsCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<AddActionsCommand>
{
    public async Task Handle(AddActionsCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindAsync<ServiceAggregate>(request.IdService, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        foreach (var action in request.Actions)
        {
            ApplicationGuard.IsNull(action, Errors.InvalidRequest);

            service.AddAction(request.IdController, action.Id, action.Name, action.Description, action.HttpMethod, user.IdUser);
        }

        await repository.UpdateAsync(service, cancellationToken);

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }
}