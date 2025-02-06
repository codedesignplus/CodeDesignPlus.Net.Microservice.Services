using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceById;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Queries.GetServiceById;

public class GetServiceByIdQueryHandlerTest
{
    private readonly Mock<IServiceRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<ICacheManager> cacheManagerMock;
    private readonly GetServiceByIdQueryHandler handler;

    public GetServiceByIdQueryHandlerTest()
    {
        repositoryMock = new Mock<IServiceRepository>();
        mapperMock = new Mock<IMapper>();
        cacheManagerMock = new Mock<ICacheManager>();
        handler = new GetServiceByIdQueryHandler(repositoryMock.Object, mapperMock.Object, cacheManagerMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        GetServiceByIdQuery request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ServiceExistsInCache_ReturnsServiceDtoFromCache()
    {
        // Arrange
        var request = new GetServiceByIdQuery(Guid.NewGuid());
        var serviceDto = new ServiceDto()
        {
            Id = request.Id,
            Name = "TestService",
            Description = "Test Description"
        };
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
        cacheManagerMock.Setup(x => x.GetAsync<ServiceDto>(request.Id.ToString())).ReturnsAsync(serviceDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(serviceDto, result);
        repositoryMock.Verify(x => x.FindAsync<ServiceAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ServiceNotInCache_ServiceNotFound_ThrowsServiceNotFoundException()
    {
        // Arrange
        var request = new GetServiceByIdQuery(Guid.NewGuid());
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<ServiceAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((ServiceAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.ServiceNotFound.GetCode(), exception.Code);
        Assert.Equal(Errors.ServiceNotFound.GetMessage(), exception.Message);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ServiceNotInCache_ServiceFound_ReturnsServiceDto()
    {
        // Arrange
        var idUser = Guid.NewGuid();
        var request = new GetServiceByIdQuery(Guid.NewGuid());
        var service = ServiceAggregate.Create(Guid.NewGuid(), "TestService", "Test Description", idUser);
        var serviceDto = new ServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description
        };
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<ServiceAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(service);
        mapperMock.Setup(x => x.Map<ServiceDto>(service)).Returns(serviceDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(serviceDto, result);
        cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), serviceDto, It.IsAny<TimeSpan?>()), Times.Once);
    }
}
