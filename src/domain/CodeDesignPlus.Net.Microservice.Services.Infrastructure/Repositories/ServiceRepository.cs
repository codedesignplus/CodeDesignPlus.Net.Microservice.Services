
using CodeDesignPlus.Net.Microservice.Services.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Services.Infrastructure.Repositories;

public class ServiceRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<ServiceRepository> logger)

    : RepositoryBase(serviceProvider, mongoOptions, logger), IServiceRepository
{
    public async Task<ServiceAggregate> FindServiceByNameAsync(string name, CancellationToken cancellationToken)
    {
        var collection = base.GetCollection<ServiceAggregate>();

        var filter = Builders<ServiceAggregate>.Filter.Eq(x => x.Name, name);

        var cursor = await collection.FindAsync(filter, cancellationToken: cancellationToken);

        var service = await cursor.FirstOrDefaultAsync(cancellationToken);

        return service;
    }
}