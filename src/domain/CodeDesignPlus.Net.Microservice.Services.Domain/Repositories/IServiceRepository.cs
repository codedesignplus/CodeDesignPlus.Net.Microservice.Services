namespace CodeDesignPlus.Net.Microservice.Services.Domain.Repositories;

public interface IServiceRepository : IRepositoryBase
{
    Task<ServiceAggregate> FindServiceByNameAsync(string name, CancellationToken cancellationToken);
}