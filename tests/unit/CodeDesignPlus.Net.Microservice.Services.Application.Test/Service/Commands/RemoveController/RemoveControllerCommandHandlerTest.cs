using System;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RemoveController;
using CodeDesignPlus.Net.Microservice.Services.Domain;
using CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.RemoveController;

public class RemoveControllerCommandHandlerTest
{
    private readonly Mock<IServiceRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly RemoveControllerCommandHandler handler;

    public RemoveControllerCommandHandlerTest()
    {
        repositoryMock = new Mock<IServiceRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new RemoveControllerCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        RemoveControllerCommand request = null!;
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
        var request = new RemoveControllerCommand(Guid.NewGuid(), Guid.NewGuid());
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(r => r.FindAsync<ServiceAggregate>(request.IdService, cancellationToken))
            .ReturnsAsync((ServiceAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.ServiceNotFound.GetCode(), exception.Code);
        Assert.Equal(Errors.ServiceNotFound.GetMessage(), exception.Message);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_RemovesControllerAndPublishesEvents()
    {
        // Arrange
        var idService = Guid.NewGuid();
        var idController = Guid.NewGuid();
        var idUser = Guid.NewGuid();

        var request = new RemoveControllerCommand(idService, idController);
        var cancellationToken = CancellationToken.None;

        var service = ServiceAggregate.Create(idService, "TestService", "Test Description", idUser);
        service.AddController(idController, "TestController", "Test Description", idUser);

        repositoryMock
            .Setup(repo => repo.FindAsync<ServiceAggregate>(request.IdService, cancellationToken))
            .ReturnsAsync(service);

        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(repo => repo.UpdateAsync(service, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<ControllerRemovedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
