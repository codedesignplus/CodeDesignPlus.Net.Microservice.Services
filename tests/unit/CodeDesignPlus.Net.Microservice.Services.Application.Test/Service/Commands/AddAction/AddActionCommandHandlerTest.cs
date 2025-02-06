using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddAction;
using CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.AddAction;

public class AddActionCommandHandlerTest
{
    private readonly Mock<IServiceRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly AddActionCommandHandler handler;

    public AddActionCommandHandlerTest()
    {
        repositoryMock = new Mock<IServiceRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new AddActionCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        AddActionCommand request = null!;
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
        var request = new AddActionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "TestAction", "TestDescription", Domain.Enums.HttpMethodEnum.GET);

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
        var cancellationToken = CancellationToken.None;
        var request = new AddActionCommand(idService, idController, idAction, "TestAction", "TestDescription", Domain.Enums.HttpMethodEnum.GET);

        var service = ServiceAggregate.Create(Guid.NewGuid(), "TestService", "Test Description", idUser);
        service.AddController(idController, "TestController", "Test Description", idUser);

        repositoryMock
            .Setup(repo => repo.FindAsync<ServiceAggregate>(request.IdService, cancellationToken))
            .ReturnsAsync(service);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        var controller = service.Controllers.First(x => x.Id == idController);
        var action = controller.Actions.First(x => x.Id == idAction);

        Assert.Equal(request.Name, action.Name);
        Assert.Equal(request.Description, action.Description);
        Assert.Equal(request.HttpMethod, action.HttpMethod);

        repositoryMock.Verify(repo => repo.UpdateAsync(service, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<ActionAddedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
