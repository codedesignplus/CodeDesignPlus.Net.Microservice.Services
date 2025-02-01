namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceById;

public record GetServiceByIdQuery(Guid Id) : IRequest<ServiceDto>;

