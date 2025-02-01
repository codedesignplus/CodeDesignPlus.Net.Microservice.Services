namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.DeleteService;

public class DeleteServiceCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteServiceCommand>
{
    public Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}