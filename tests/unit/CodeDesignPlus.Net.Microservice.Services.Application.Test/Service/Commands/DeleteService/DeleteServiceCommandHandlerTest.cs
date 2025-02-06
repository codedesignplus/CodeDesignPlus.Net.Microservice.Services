using System;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.DeleteService;
using CodeDesignPlus.Net.Microservice.Services.Domain;
using CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.DeleteService;

public class DeleteServiceCommandHandlerTest
{
    private readonly Mock<IServiceRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly DeleteServiceCommandHandler handler;

    public DeleteServiceCommandHandlerTest()
    {
        repositoryMock = new Mock<IServiceRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new DeleteServiceCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        DeleteServiceCommand request = null!;
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
        var request = new DeleteServiceCommand(Guid.NewGuid());
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(r => r.FindAsync<ServiceAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((ServiceAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.ServiceNotFound.GetCode(), exception.Code);
        Assert.Equal(Errors.ServiceNotFound.GetMessage(), exception.Message);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesServiceAndPublishesEvents()
    {
        // Arrange
        var idUser = Guid.NewGuid();
        var idService = Guid.NewGuid();
        var request = new DeleteServiceCommand(idService);
        var cancellationToken = CancellationToken.None;
        var service = ServiceAggregate.Create(idService, "TestService", "Test Description", idUser);

        userContextMock.SetupGet(u => u.IdUser).Returns(Guid.NewGuid());
        
        repositoryMock
            .Setup(repo => repo.FindAsync<ServiceAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(service);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert        
        repositoryMock.Verify(repo => repo.DeleteAsync<ServiceAggregate>(request.Id, It.IsAny<Guid>(), cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<ServiceDeletedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
