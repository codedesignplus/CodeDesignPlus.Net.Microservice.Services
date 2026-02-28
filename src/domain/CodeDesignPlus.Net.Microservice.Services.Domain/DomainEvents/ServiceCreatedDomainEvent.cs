namespace CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

[EventKey<ServiceAggregate>(1, "ServiceCreatedDomainEvent")]
public class ServiceCreatedDomainEvent(
    Guid aggregateId,
    string name, 
    string description, 
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;

    public string Description { get; private set; } = description;

    public bool IsActive { get; private set; } = isActive;

    public static ServiceCreatedDomainEvent Create(Guid aggregateId, string name, string description, bool isActive)
    {
        return new ServiceCreatedDomainEvent(aggregateId, name, description, isActive);
    }
}
