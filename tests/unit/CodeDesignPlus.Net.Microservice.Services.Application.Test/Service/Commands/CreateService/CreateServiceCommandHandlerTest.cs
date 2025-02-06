using System;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.CreateService;
using CodeDesignPlus.Net.Microservice.Services.Domain;
using CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.CreateService;

public class CreateServiceCommandHandlerTest
{
    private readonly Mock<IServiceRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly CreateServiceCommandHandler handler;

    public CreateServiceCommandHandlerTest()
    {
        repositoryMock = new Mock<IServiceRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new CreateServiceCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        CreateServiceCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ServiceAlreadyExists_ThrowsServiceAlreadyExistsException()
    {
        // Arrange
        var request = new CreateServiceCommand(Guid.NewGuid(), "Test Service", "Test Description");
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(x => x.ExistsAsync<ServiceAggregate>(request.Id, cancellationToken)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.ServiceAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Errors.ServiceAlreadyExists.GetMessage(), exception.Message);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesServiceAndPublishesEvents()
    {
        // Arrange
        var tenant = Guid.NewGuid();
        var request = new CreateServiceCommand(Guid.NewGuid(), "Test Service", "Test Description");
        var cancellationToken = CancellationToken.None;

        userContextMock.Setup(x => x.Tenant).Returns(tenant);
        userContextMock.Setup(x => x.IdUser).Returns(Guid.NewGuid());
        repositoryMock.Setup(x => x.ExistsAsync<ServiceAggregate>(request.Id, tenant, cancellationToken)).ReturnsAsync(false);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(x => x.CreateAsync(It.IsAny<ServiceAggregate>(), cancellationToken), Times.Once);
        pubSubMock.Verify(x => x.PublishAsync(It.IsAny<List<ServiceCreatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
