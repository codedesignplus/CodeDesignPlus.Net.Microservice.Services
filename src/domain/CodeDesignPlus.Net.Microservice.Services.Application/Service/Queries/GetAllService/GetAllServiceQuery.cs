namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetAllService;

public record GetAllServiceQuery(Guid Id) : IRequest<ServiceDto>;

