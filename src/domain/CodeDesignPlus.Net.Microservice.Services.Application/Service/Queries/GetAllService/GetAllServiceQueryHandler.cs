namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetAllService;

public class GetAllServiceQueryHandler(IServiceRepository repository, IMapper mapper) : IRequestHandler<GetAllServiceQuery, List<ServiceDto>>
{
    public async Task<List<ServiceDto>> Handle(GetAllServiceQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var tenants = await repository.MatchingAsync<ServiceAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<ServiceDto>>(tenants);
    }
}
