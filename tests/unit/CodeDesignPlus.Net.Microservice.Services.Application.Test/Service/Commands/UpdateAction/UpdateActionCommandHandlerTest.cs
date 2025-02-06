using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateAction;
using CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;
using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.UpdateAction;

public class UpdateActionCommandHandlerTest
{
    private readonly Mock<IServiceRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly UpdateActionCommandHandler handler;

    public UpdateActionCommandHandlerTest()
    {
        repositoryMock = new Mock<IServiceRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new UpdateActionCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        UpdateActionCommand request = null!;
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
        var request = new UpdateActionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "TestAction", "TestDescription", HttpMethodEnum.GET);
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
        var idAction = Guid.NewGuid();
        var idUser = Guid.NewGuid();

        var request = new UpdateActionCommand(idService, idController, idAction, "TestAction", "TestDescription", HttpMethodEnum.GET);
        var cancellationToken = CancellationToken.None;

        var service = ServiceAggregate.Create(Guid.NewGuid(), "TestService", "Test Description", idUser);
        service.AddController(idController, "TestController", "Test Description", idUser);
        service.AddAction(idController, idAction, "TestAction", "Test Description", HttpMethodEnum.POST, idUser);

        repositoryMock
            .Setup(repo => repo.FindAsync<ServiceAggregate>(request.IdService, cancellationToken))
            .ReturnsAsync(service);

        userContextMock.SetupGet(user => user.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        var controller = service.Controllers.First(x => x.Id == idController);
        var action = controller.Actions.First(x => x.Id == idAction);

        
        repositoryMock.Verify(repo => repo.UpdateAsync(service, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<ActionUpdatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
