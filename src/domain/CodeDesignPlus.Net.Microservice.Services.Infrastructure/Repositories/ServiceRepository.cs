namespace CodeDesignPlus.Net.Microservice.Services.Infrastructure.Repositories;

public class ServiceRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<ServiceRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IServiceRepository
{
   
}