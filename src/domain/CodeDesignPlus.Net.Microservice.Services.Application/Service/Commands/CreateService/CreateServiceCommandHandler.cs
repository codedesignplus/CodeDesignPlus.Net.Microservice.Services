namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.CreateService;

public class CreateServiceCommandHandler(IServiceRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateServiceCommand>
{
    public Task Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}