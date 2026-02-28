namespace CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

[EventKey<ServiceAggregate>(1, "ControllerAddedDomainEvent")]
public class ControllerAddedDomainEvent(
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

    public static ControllerAddedDomainEvent Create(Guid aggregateId, Guid idController, string name, string description)
    {
        return new ControllerAddedDomainEvent(aggregateId, idController, name, description);
    }
}
