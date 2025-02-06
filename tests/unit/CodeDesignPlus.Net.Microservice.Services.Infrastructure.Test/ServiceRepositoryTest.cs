namespace CodeDesignPlus.Net.Microservice.Services.Infrastructure.Test;

public class ServiceRepositoryTest
{
    private readonly Mock<IServiceProvider> serviceProviderMock;
    private readonly Mock<IOptions<MongoOptions>> mongoOptionsMock;
    private readonly Mock<ILogger<ServiceRepository>> loggerMock;

    public ServiceRepositoryTest()
    {
        serviceProviderMock = new Mock<IServiceProvider>();
        mongoOptionsMock = new Mock<IOptions<MongoOptions>>();
        loggerMock = new Mock<ILogger<ServiceRepository>>();
    }

    [Fact]
    public void Constructor_ShouldInitializeServiceRepository()
    {
        // Arrange
        var mongoOptions = new MongoOptions();
        mongoOptionsMock.Setup(m => m.Value).Returns(mongoOptions);

        // Act
        var repository = new ServiceRepository(serviceProviderMock.Object, mongoOptionsMock.Object, loggerMock.Object);

        // Assert
        Assert.NotNull(repository);
    }
}
