namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceByName;

public class GetServiceByNameQueryHandler(IServiceRepository repository, IMapper mapper) : IRequestHandler<GetServiceByNameQuery, ServiceDto>
{
    public async Task<ServiceDto> Handle(GetServiceByNameQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindServiceByNameAsync(request.Name, cancellationToken);

        ApplicationGuard.IsNull(service, Errors.ServiceNotFound);

        return mapper.Map<ServiceDto>(service);
    }
}
