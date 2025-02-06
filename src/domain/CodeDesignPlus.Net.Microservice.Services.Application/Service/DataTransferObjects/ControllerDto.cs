namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.DataTransferObjects;

public class ControllerDto: IDtoBase
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public required string Description { get; set; } 
    public List<ActionDto> Actions { get; set; } = [];
}
