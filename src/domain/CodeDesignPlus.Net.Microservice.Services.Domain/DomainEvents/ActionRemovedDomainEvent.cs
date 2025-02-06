using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

[EventKey<ServiceAggregate>(1, "ActionRemovedDomainEvent")]
public class ActionRemovedDomainEvent(
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
    public static ActionRemovedDomainEvent Create(Guid aggregateId, Guid idController, Guid idAction, string name, string description, Enums.HttpMethod httpMethod)
    {
        return new ActionRemovedDomainEvent(aggregateId, idController, idAction, name, description, httpMethod);
    }
}
