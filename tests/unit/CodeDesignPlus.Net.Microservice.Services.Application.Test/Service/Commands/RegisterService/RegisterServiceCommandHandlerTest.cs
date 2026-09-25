using System;
using System.Collections.Generic;
using System.Linq;
using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RegisterService;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Services.Domain;
using HttpMethod = CodeDesignPlus.Net.Microservice.Services.Domain.Enums.HttpMethod;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.RegisterService;

/// <summary>
/// El registro al arrancar se resuelve en una sola escritura, sin leer de la cache (pendings/018), y actualiza la
/// descripcion cuando cambia (pendings/016).
/// </summary>
public class RegisterServiceCommandHandlerTest
{
    private readonly Mock<IServiceRepository> repository = new();
    private readonly Mock<IPubSub> pubsub = new();
    private readonly Mock<ICacheManager> cache = new();
    private readonly RegisterServiceCommandHandler handler;

    public RegisterServiceCommandHandlerTest()
    {
        handler = new RegisterServiceCommandHandler(repository.Object, pubsub.Object, cache.Object);
    }

    private static RegisterServiceCommand Command(Guid id, string description = "Accounting") => new(id, "ms-accounting", description,
    [
        new ControllerDto
        {
            Id = Guid.NewGuid(), Name = "AccountantController", Description = "Accountants",
            Actions = [ new ActionDto { Id = Guid.NewGuid(), Name = "GetAll", Description = "List", HttpMethod = HttpMethod.GET } ]
        }
    ]);

    [Fact]
    public async Task Handle_ServicioNuevo_LoCreaEnUnaEscrituraConControllersYAcciones()
    {
        var command = Command(Guid.NewGuid());
        ServiceAggregate creado = null!;
        repository.Setup(x => x.CreateAsync(It.IsAny<ServiceAggregate>(), It.IsAny<CancellationToken>()))
            .Callback<ServiceAggregate, CancellationToken>((s, _) => creado = s);

        await handler.Handle(command, CancellationToken.None);

        repository.Verify(x => x.CreateAsync(It.IsAny<ServiceAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.UpdateAsync(It.IsAny<ServiceAggregate>(), It.IsAny<CancellationToken>()), Times.Never);
        var controller = Assert.Single(creado.Controllers);
        Assert.Equal("Accountant", controller.Name);
        Assert.Equal("GetAll", Assert.Single(controller.Actions).Name);
        cache.Verify(x => x.RemoveAsync(command.Id.ToString()), Times.Once);
    }

    [Fact]
    public async Task Handle_NoConsultaLaCache()
    {
        await handler.Handle(Command(Guid.NewGuid()), CancellationToken.None);

        cache.Verify(x => x.ExistsAsync(It.IsAny<string>()), Times.Never);
        cache.Verify(x => x.GetAsync<ServiceDto>(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ServicioExistente_ActualizaDescripcionYAgregaSoloLoNuevo()
    {
        var id = Guid.NewGuid();
        var existente = ServiceAggregate.Create(id, "ms-accounting", "<microservice_description>", Guid.Empty);
        existente.AddController(Guid.NewGuid(), "Accountant", "Accountants", Guid.Empty);
        repository.Setup(x => x.FindAsync<ServiceAggregate>(id, It.IsAny<CancellationToken>())).ReturnsAsync(existente);

        await handler.Handle(Command(id, "Accounting and taxes"), CancellationToken.None);

        repository.Verify(x => x.UpdateAsync(existente, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal("Accounting and taxes", existente.Description);
        var controller = Assert.Single(existente.Controllers);
        Assert.Equal("GetAll", Assert.Single(controller.Actions).Name);
    }

    [Fact]
    public async Task Handle_OtraReplicaLoCreoPrimero_SeAplicaSobreElExistente()
    {
        var id = Guid.NewGuid();
        var delOtro = ServiceAggregate.Create(id, "ms-accounting", "Accounting", Guid.Empty);
        repository.SetupSequence(x => x.FindAsync<ServiceAggregate>(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ServiceAggregate)null!)
            .ReturnsAsync(delOtro);
        repository.Setup(x => x.CreateAsync(It.IsAny<ServiceAggregate>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("duplicate key"));
        repository.Setup(x => x.ExistsAsync<ServiceAggregate>(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        await handler.Handle(Command(id), CancellationToken.None);

        repository.Verify(x => x.UpdateAsync(delOtro, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Single(delOtro.Controllers);
    }

    [Fact]
    public async Task Handle_FallaAlCrearYNoExiste_PropagaElError()
    {
        var id = Guid.NewGuid();
        repository.Setup(x => x.CreateAsync(It.IsAny<ServiceAggregate>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("mongo caido"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(Command(id), CancellationToken.None));
    }
}
