using System;
using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.DeleteService;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;
using CodeDesignPlus.Net.Microservice.Services.Domain;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands;

/// <summary>
/// Toda escritura borra la entrada de GetServiceById; si no, el detalle sirve la version anterior (pendings/018).
/// </summary>
public class CacheInvalidationTest
{
    private readonly Mock<IServiceRepository> repository = new();
    private readonly Mock<IUserContext> user = new();
    private readonly Mock<IPubSub> pubsub = new();
    private readonly Mock<ICacheManager> cache = new();

    [Fact]
    public async Task UpdateService_BorraLaCacheDelServicio()
    {
        var service = ServiceAggregate.Create(Guid.NewGuid(), "ms-x", "X", Guid.NewGuid());
        repository.Setup(x => x.FindAsync<ServiceAggregate>(service.Id, It.IsAny<CancellationToken>())).ReturnsAsync(service);
        var handler = new UpdateServiceCommandHandler(repository.Object, user.Object, pubsub.Object, cache.Object);

        await handler.Handle(new UpdateServiceCommand(service.Id, "ms-x", "Y", false), CancellationToken.None);

        cache.Verify(x => x.RemoveAsync(service.Id.ToString()), Times.Once);
    }

    [Fact]
    public async Task DeleteService_BorraLaCacheDelServicio()
    {
        var service = ServiceAggregate.Create(Guid.NewGuid(), "ms-x", "X", Guid.NewGuid());
        repository.Setup(x => x.FindAsync<ServiceAggregate>(service.Id, It.IsAny<CancellationToken>())).ReturnsAsync(service);
        var handler = new DeleteServiceCommandHandler(repository.Object, user.Object, pubsub.Object, cache.Object);

        await handler.Handle(new DeleteServiceCommand(service.Id), CancellationToken.None);

        cache.Verify(x => x.RemoveAsync(service.Id.ToString()), Times.Once);
    }
}
