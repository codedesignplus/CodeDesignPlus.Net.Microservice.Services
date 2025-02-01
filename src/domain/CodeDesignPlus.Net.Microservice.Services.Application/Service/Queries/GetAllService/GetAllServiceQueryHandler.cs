namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetAllService;

public class GetAllServiceQueryHandler(IServiceRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<GetAllServiceQuery, ServiceDto>
{
    public Task<ServiceDto> Handle(GetAllServiceQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<ServiceDto>(default!);
    }
}
