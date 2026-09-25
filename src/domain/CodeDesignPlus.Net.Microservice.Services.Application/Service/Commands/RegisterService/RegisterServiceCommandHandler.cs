namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RegisterService;

/// <summary>
/// Registra un microservicio en el catalogo en una sola escritura (pendings/018).
/// </summary>
/// <remarks>
/// Antes el registro eran tres comandos (crear, agregar controllers, agregar acciones) y cada uno volvia a leer lo que
/// acababa de escribir el anterior, pasando ademas por la cache de <c>GetServiceById</c>. Si la cache tenia un servicio
/// que ya no estaba en Mongo, o la lectura no veia aun la escritura, el registro fallaba con <c>ServiceNotFound</c> y el
/// micro que se registraba se caia. Aqui se lee del repositorio, se arma el agregado entero en memoria y se guarda una
/// vez. Si el servicio ya existe y cambio su nombre o su descripcion (<c>Core:Description</c>), se actualizan: antes
/// el catalogo conservaba para siempre la del primer registro (pendings/016).
/// </remarks>
public class RegisterServiceCommandHandler(IServiceRepository repository, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<RegisterServiceCommand>
{
    public async Task Handle(RegisterServiceCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var service = await repository.FindAsync<ServiceAggregate>(request.Id, cancellationToken);

        if (service is null)
        {
            service = ServiceAggregate.Create(request.Id, request.Name, request.Description, Guid.Empty);
            Apply(service, request);

            try
            {
                await repository.CreateAsync(service, cancellationToken);
            }
            catch (Exception)
            {
                // Dos replicas del mismo micro arrancaron a la vez y la otra lo creo primero: se aplica sobre el suyo.
                // Si no fue eso, el error es otro y se propaga.
                if (!await repository.ExistsAsync<ServiceAggregate>(request.Id, cancellationToken))
                    throw;

                service = await repository.FindAsync<ServiceAggregate>(request.Id, cancellationToken);
                Apply(service, request);
                await repository.UpdateAsync(service, cancellationToken);
            }
        }
        else
        {
            if (service.Name != request.Name || service.Description != request.Description)
                service.Update(request.Name, request.Description, service.IsActive, Guid.Empty);

            Apply(service, request);

            await repository.UpdateAsync(service, cancellationToken);
        }

        await cacheManager.RemoveAsync(request.Id.ToString());

        await pubsub.PublishAsync(service.GetAndClearEvents(), cancellationToken);
    }

    /// <summary>Agrega los controllers y acciones que falten. El agregado ignora los que ya existen por nombre.</summary>
    private static void Apply(ServiceAggregate service, RegisterServiceCommand request)
    {
        foreach (var controller in request.Controllers ?? [])
        {
            service.AddController(controller.Id, controller.Name, controller.Description ?? string.Empty, Guid.Empty);

            foreach (var action in controller.Actions ?? [])
                service.AddAction(controller.Id, controller.Name, action.Id, action.Name, action.Description ?? string.Empty, action.HttpMethod, Guid.Empty);
        }
    }
}
