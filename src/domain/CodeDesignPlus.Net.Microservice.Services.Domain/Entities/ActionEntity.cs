using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Services.Domain.Entities;

public class ActionEntity : IEntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Enums.HttpMethod HttpMethod { get; set; } = Enums.HttpMethod.None; 
}
