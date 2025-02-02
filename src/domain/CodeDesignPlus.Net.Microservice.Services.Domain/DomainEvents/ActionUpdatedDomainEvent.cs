using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

[EventKey<ServiceAggregate>(1, "ActionUpdatedDomainEvent")]
public class ActionUpdatedDomainEvent(
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
    public Guid IdController { get; private set; } = idController;

    public Guid IdAction { get; private set; } = idAction;

    public string Name { get; private set; } = name;

    public string Description { get; private set; } = description;

    public HttpMethodEnum HttpMethod { get; private set; } = httpMethod;
    
    public static ActionUpdatedDomainEvent Create(Guid aggregateId, Guid idController, Guid idAction, string name, string description, HttpMethodEnum httpMethod)
    {
        return new ActionUpdatedDomainEvent(aggregateId, idController, idAction, name, description, httpMethod);
    }
}
