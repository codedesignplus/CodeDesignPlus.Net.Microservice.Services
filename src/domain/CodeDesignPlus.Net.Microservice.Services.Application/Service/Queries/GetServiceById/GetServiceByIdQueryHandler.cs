namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceById;

public class GetServiceByIdQueryHandler(IServiceRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<GetServiceByIdQuery, ServiceDto>
{
    public async Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<ServiceDto>(request.Id.ToString());

        var service = await repository.FindAsync<ServiceAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<ServiceDto>(service));

        return mapper.Map<ServiceDto>(service);
    }
}
