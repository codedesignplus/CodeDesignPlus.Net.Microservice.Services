namespace CodeDesignPlus.Net.Microservice.Services.Rest.Controllers;

/// <summary>
/// Controller for managing the Services.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class ServiceController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Services.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Services.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Services.</returns>
    [HttpGet]
    public async Task<IActionResult> GetServices([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllServiceQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a Service by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Service.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Service.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetServiceById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetServiceByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new Service.
    /// </summary>
    /// <param name="data">Data for creating the Service.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateServiceCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Add a new Controller to an existing Service.
    /// </summary>
    /// <param name="id">The unique identifier of the Service.</param>
    /// <param name="data">Data for creating the Controller.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost("{id}/controller")]
    public async Task<IActionResult> AddController(Guid id, [FromBody] AddControllerDto data, CancellationToken cancellationToken)
    {
        data.IdService = id;

        await mediator.Send(mapper.Map<AddControllerCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Controller.
    /// </summary>
    /// <param name="id">The unique identifier of the Service.</param>
    /// <param name="idController">The unique identifier of the Controller.</param>
    /// <param name="data">Data for updating the Controller.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost("{id}/controller/{idController}/action")]
    public async Task<IActionResult> AddAction(Guid id, Guid idController, [FromBody] AddActionDto data, CancellationToken cancellationToken)
    {
        data.IdService = id;
        data.IdController = idController;

        await mediator.Send(mapper.Map<AddActionCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Service.
    /// </summary>
    /// <param name="id">The unique identifier of the Service.</param>
    /// <param name="data">Data for updating the Service.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(Guid id, [FromBody] UpdateServiceDto data, CancellationToken cancellationToken)
    {
        data.Id = id;

        await mediator.Send(mapper.Map<UpdateServiceCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Controller.
    /// </summary>
    /// <param name="id">The unique identifier of the Service.</param>
    /// <param name="idController">The unique identifier of the Controller.</param>
    /// <param name="data">Data for updating the Controller.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}/controller/{idController}")]
    public async Task<IActionResult> UpdateController(Guid id, Guid idController, [FromBody] UpdateControllerDto data, CancellationToken cancellationToken)
    {
        data.IdService = id;
        data.IdController = idController;

        await mediator.Send(mapper.Map<UpdateControllerCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Action.
    /// </summary>
    /// <param name="id">The unique identifier of the Service.</param>
    /// <param name="idController">The unique identifier of the Controller.</param>
    /// <param name="idAction">The unique identifier of the Action.</param>
    /// <param name="data">Data for updating the Action.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}/controller/{idController}/action/{idAction}")]
    public async Task<IActionResult> UpdateAction(Guid id, Guid idController, Guid idAction, [FromBody] UpdateActionDto data, CancellationToken cancellationToken)
    {
        data.IdService = id;
        data.IdController = idController;
        data.IdAction = idAction;

        await mediator.Send(mapper.Map<UpdateActionCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing Service.
    /// </summary>
    /// <param name="id">The unique identifier of the Service.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteServiceCommand(id), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing Controller.
    /// </summary>
    /// <param name="id">The unique identifier of the Service.</param>
    /// <param name="idController">The unique identifier of the Controller.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}/controller/{idController}")]
    public async Task<IActionResult> RemoveController(Guid id, Guid idController, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveControllerCommand(id, idController), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing Action.
    /// </summary>
    /// <param name="id">The unique identifier of the Service.</param>
    /// <param name="idController">The unique identifier of the Controller.</param>
    /// <param name="idAction">The unique identifier of the Action.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}/controller/{idController}/action/{idAction}")]
    public async Task<IActionResult> RemoveAction(Guid id, Guid idController, Guid idAction, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveActionCommand(id, idController, idAction), cancellationToken);

        return NoContent();
    }
}