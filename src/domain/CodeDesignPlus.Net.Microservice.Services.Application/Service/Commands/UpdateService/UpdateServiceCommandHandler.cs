namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;

public class UpdateServiceCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateServiceCommand>
{
    public Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}