namespace CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

[EventKey<ServiceAggregate>(1, "ControllerUpdatedDomainEvent")]
public class ControllerUpdatedDomainEvent(
     Guid aggregateId,
     Guid idController, 
     string name, 
     string description,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public Guid IdController { get; private set; } = idController;

    public string Name { get; private set; } = name;

    public string Description { get; private set; } = description;

    public static ControllerUpdatedDomainEvent Create(Guid aggregateId, Guid idController, string name, string description)
    {
        return new ControllerUpdatedDomainEvent(aggregateId, idController, name, description);
    }
}
