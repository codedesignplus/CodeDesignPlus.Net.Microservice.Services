using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.DataTransferObjects;

public class ActionDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public required string Description { get; set; } 
    public HttpMethodEnum HttpMethod { get; set; } 
}
