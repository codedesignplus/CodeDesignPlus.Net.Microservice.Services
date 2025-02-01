namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceById;

public class GetServiceByIdQueryHandler(IServiceRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<GetServiceByIdQuery, ServiceDto>
{
    public Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<ServiceDto>(default!);
    }
}
