namespace CodeDesignPlus.Net.Microservice.Services.Domain.Entities;

public class ControllerEntity : IEntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ActionEntity> Actions { get; set; } = [];
}
