using System;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateController;
using CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.UpdateController;

public class UpdateControllerCommandHandlerTest
{
    private readonly Mock<IServiceRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly UpdateControllerCommandHandler handler;

    public UpdateControllerCommandHandlerTest()
    {
        repositoryMock = new Mock<IServiceRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new UpdateControllerCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        UpdateControllerCommand request = null!;
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
        var request = new UpdateControllerCommand(Guid.NewGuid(), Guid.NewGuid(), "TestController", "TestDescription");
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(repo => repo.FindAsync<ServiceAggregate>(request.IdService, cancellationToken))
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
        var idController = Guid.NewGuid();
        var idUser = Guid.NewGuid();

        var request = new UpdateControllerCommand(idService, idController,  "TestController", "TestDescription");
        var cancellationToken = CancellationToken.None;

        var service = ServiceAggregate.Create(Guid.NewGuid(), "TestService", "Test Description", idUser);
        service.AddController(idController, "TestController", "Test Description", idUser);

        repositoryMock
            .Setup(repo => repo.FindAsync<ServiceAggregate>(request.IdService, cancellationToken))
            .ReturnsAsync(service);

        userContextMock.SetupGet(user => user.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        var controller = service.Controllers.First(x => x.Id == idController);
        
        Assert.Equal(request.Name, controller.Name);
        Assert.Equal(request.Description, controller.Description);
        
        repositoryMock.Verify(repo => repo.UpdateAsync(service, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<ControllerUpdatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
