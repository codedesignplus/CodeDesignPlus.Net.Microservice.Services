namespace CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

[EventKey<ServiceAggregate>(1, "ActionAddedDomainEvent")]
public class ActionAddedDomainEvent(
    Guid aggregateId,
    Guid idController, 
    Guid idAction,
    string name, 
    string description,
    Enums.HttpMethod httpMethod,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : ActionDomainEventBase(aggregateId, idController, idAction, name, description, httpMethod, eventId, occurredAt, metadata)
{
    public static ActionAddedDomainEvent Create(Guid aggregateId, Guid idController, Guid idAction, string name, string description, Enums.HttpMethod httpMethod)
    {
        return new ActionAddedDomainEvent(aggregateId, idController, idAction, name, description, httpMethod);
    }
}
