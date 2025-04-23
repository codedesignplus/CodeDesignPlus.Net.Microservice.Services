using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetAllService;

public class GetAllServiceQueryHandler(IServiceRepository repository, IMapper mapper) : IRequestHandler<GetAllServiceQuery, Pagination<ServiceDto>>
{
    public async Task<Pagination<ServiceDto>> Handle(GetAllServiceQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var tenants = await repository.MatchingAsync<ServiceAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<ServiceDto>>(tenants);
    }
}
