using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

[EventKey<ServiceAggregate>(1, "ActionRemovedDomainEvent")]
public class ActionRemovedDomainEvent(
    Guid aggregateId,
    Guid idController, 
    Guid idAction,
    string name, 
    string description, 
    HttpMethodEnum httpMethod,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static ActionRemovedDomainEvent Create(Guid aggregateId, Guid idController, Guid idAction, string name, string description, HttpMethodEnum httpMethod)
    {
        return new ActionRemovedDomainEvent(aggregateId, idController, idAction, name, description, httpMethod);
    }
}
