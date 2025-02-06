using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetAllService;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceById;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.CreateService;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddController;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddAction;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateController;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateAction;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.DeleteService;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RemoveController;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RemoveAction;

namespace CodeDesignPlus.Net.Microservice.Services.Rest.Test.Controllers;

public class ServiceControllerTest
{
    private readonly Mock<IMediator> mediatorMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly ServiceController controller;

    public ServiceControllerTest()
    {
        mediatorMock = new Mock<IMediator>();
        mapperMock = new Mock<IMapper>();
        controller = new ServiceController(mediatorMock.Object, mapperMock.Object);
    }

    [Fact]
    public async Task GetServices_ReturnsOkResult()
    {
        // Arrange
        var criteria = new C.Criteria();
        var cancellationToken = new CancellationToken();
        var services = new List<ServiceDto>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllServiceQuery>(), cancellationToken)).ReturnsAsync(services);

        // Act
        var result = await controller.GetServices(criteria, cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(services, okResult.Value);

        mediatorMock.Verify(m => m.Send(It.IsAny<GetAllServiceQuery>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetServiceById_ReturnsOkResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var service = new ServiceDto()
        {
            Id = id,
            Name = "TestService",
            Description = "Test Description"
        };
        mediatorMock.Setup(m => m.Send(It.IsAny<GetServiceByIdQuery>(), cancellationToken)).ReturnsAsync(service);

        // Act
        var result = await controller.GetServiceById(id, cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(service, okResult.Value);

        mediatorMock.Verify(m => m.Send(It.IsAny<GetServiceByIdQuery>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CreateService_ReturnsNoContentResult()
    {
        // Arrange
        var data = new CreateServiceDto();
        var cancellationToken = new CancellationToken();
        var command = new CreateServiceCommand(Guid.NewGuid(), "Test Service", "Test Description");
        mapperMock.Setup(m => m.Map<CreateServiceCommand>(data)).Returns(command);
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.CreateService(data, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(command, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task AddController_ReturnsNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var data = new AddControllerDto();
        var cancellationToken = new CancellationToken();
        var command = new AddControllerCommand(id, Guid.NewGuid(), "TestController", "Test Description");
        mapperMock.Setup(m => m.Map<AddControllerCommand>(data)).Returns(command);
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.AddController(id, data, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(command, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task AddAction_ReturnsNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idController = Guid.NewGuid();
        var data = new AddActionDto();
        var cancellationToken = new CancellationToken();
        var command = new AddActionCommand(id, idController, Guid.NewGuid(), "TestAction", "Test Description", Domain.Enums.HttpMethod.GET);
        mapperMock.Setup(m => m.Map<AddActionCommand>(data)).Returns(command);
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.AddAction(id, idController, data, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(command, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateService_ReturnsNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var data = new UpdateServiceDto();
        var cancellationToken = new CancellationToken();
        var command = new UpdateServiceCommand(id, "Test Service", "Test Description", false);
        mapperMock.Setup(m => m.Map<UpdateServiceCommand>(data)).Returns(command);
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.UpdateService(id, data, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(command, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateController_ReturnsNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idController = Guid.NewGuid();
        var data = new UpdateControllerDto();
        var cancellationToken = new CancellationToken();
        var command = new UpdateControllerCommand(id, idController, "TestController", "Test Description");
        mapperMock.Setup(m => m.Map<UpdateControllerCommand>(data)).Returns(command);
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.UpdateController(id, idController, data, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(command, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateAction_ReturnsNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idController = Guid.NewGuid();
        var idAction = Guid.NewGuid();
        var data = new UpdateActionDto();
        var cancellationToken = new CancellationToken();
        var command = new UpdateActionCommand(id, idController, idAction, "TestAction", "Test Description", Domain.Enums.HttpMethod.GET);
        mapperMock.Setup(m => m.Map<UpdateActionCommand>(data)).Returns(command);
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.UpdateAction(id, idController, idAction, data, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(command, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteService_ReturnsNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var command = new DeleteServiceCommand(id);
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.DeleteService(id, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(command, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RemoveController_ReturnsNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idController = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var command = new RemoveControllerCommand(id, idController);
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.RemoveController(id, idController, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(command, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RemoveAction_ReturnsNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idController = Guid.NewGuid();
        var idAction = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var command = new RemoveActionCommand(id, idController, idAction);
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.RemoveAction(id, idController, idAction, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(command, cancellationToken), Times.Once);
    }
}
