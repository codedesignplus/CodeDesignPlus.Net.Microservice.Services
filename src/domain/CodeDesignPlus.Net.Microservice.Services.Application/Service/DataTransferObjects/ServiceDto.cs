namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.DataTransferObjects;

public class ServiceDto : IDtoBase
{
    public required Guid Id { get; set; }

    public required string Name { get; set; } 

    public required string Description { get; set; } 

    public List<ControllerDto> Controllers { get; set; } = [];

    public bool IsActive { get; set; }
}