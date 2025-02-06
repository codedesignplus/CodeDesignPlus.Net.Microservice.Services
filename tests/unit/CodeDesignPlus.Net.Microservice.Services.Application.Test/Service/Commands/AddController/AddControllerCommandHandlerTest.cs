using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddController;
using CodeDesignPlus.Net.Microservice.Services.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.AddController;

public class AddControllerCommandHandlerTest
{
    private readonly Mock<IServiceRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly AddControllerCommandHandler handler;

    public AddControllerCommandHandlerTest()
    {
        repositoryMock = new Mock<IServiceRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new AddControllerCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        AddControllerCommand request = null!;
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
        var request = new AddControllerCommand(Guid.NewGuid(), Guid.NewGuid(), "TestController", "Test Description");
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
    public async Task Handle_ValidRequest_UpdatesServiceAndPublishesEvents()
    {
        // Arrange
        var idService = Guid.NewGuid();
        var idController = Guid.NewGuid();
        var idAction = Guid.NewGuid();
        var idUser = Guid.NewGuid();
        var request = new AddControllerCommand(idService, idController, "TestController", "Test Description");
        var cancellationToken = CancellationToken.None;
        var service = ServiceAggregate.Create(idService, "TestService", "Test Description", idUser);

        repositoryMock
            .Setup(repo => repo.FindAsync<ServiceAggregate>(request.IdService, cancellationToken))
            .ReturnsAsync(service);

        userContextMock
            .SetupGet(user => user.IdUser)
            .Returns(idUser);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        var controller = service.Controllers.First(x => x.Id == idController);

        Assert.NotNull(controller);
        Assert.Equal(request.Name, controller.Name);
        Assert.Equal(request.Description, controller.Description);
        
        repositoryMock.Verify(repo => repo.UpdateAsync(service, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<ControllerAddedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
