using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetAllService;

public record GetAllServiceQuery(C.Criteria Criteria) : IRequest<Pagination<ServiceDto>>;

