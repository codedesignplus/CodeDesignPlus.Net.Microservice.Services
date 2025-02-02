namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetAllService;

public record GetAllServiceQuery(C.Criteria Criteria) : IRequest<List<ServiceDto>>;

