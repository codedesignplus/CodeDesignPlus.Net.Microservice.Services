
namespace CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

public abstract class ActionDomainEventBase(
    Guid aggregateId,
    Guid idController, 
    Guid idAction,
    string name, 
    string description,
    Enums.HttpMethod httpMethod,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public Guid IdController { get; private set; } = idController;

    public Guid IdAction { get; private set; } = idAction;

    public string Name { get; private set; } = name;

    public string Description { get; private set; } = description;

    public Enums.HttpMethod HttpMethod { get; private set; } = httpMethod;
}
