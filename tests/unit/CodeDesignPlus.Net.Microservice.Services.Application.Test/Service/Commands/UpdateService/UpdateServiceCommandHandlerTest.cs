using System;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;
using CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.UpdateService;

public class UpdateServiceCommandHandlerTest
{
    private readonly Mock<IServiceRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly UpdateServiceCommandHandler handler;

    public UpdateServiceCommandHandlerTest()
    {
        repositoryMock = new Mock<IServiceRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new UpdateServiceCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        UpdateServiceCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ServiceNotFound_ThrowsServiceNotFoundException()
    {
        // Arrange
        var request = new UpdateServiceCommand(Guid.NewGuid(), "TestService", "TestDescription", true);
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(repo => repo.FindAsync<ServiceAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((ServiceAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.ServiceNotFound.GetCode(), exception.Code);
        Assert.Equal(Errors.ServiceNotFound.GetMessage(), exception.Message);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesServiceAndPublishesEvents()
    {
        // Arrange
        var idService = Guid.NewGuid();
        var idUser = Guid.NewGuid();

        var request = new UpdateServiceCommand(idService,  "TestService", "TestDescription", false);
        var cancellationToken = CancellationToken.None;

        var service = ServiceAggregate.Create(Guid.NewGuid(), "TestService", "Test Description", idUser);

        repositoryMock
            .Setup(repo => repo.FindAsync<ServiceAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(service);

        userContextMock.SetupGet(user => user.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        
        Assert.Equal(request.Name, service.Name);
        Assert.Equal(request.Description, service.Description);
        
        repositoryMock.Verify(repo => repo.UpdateAsync(service, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<ServiceUpdatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
