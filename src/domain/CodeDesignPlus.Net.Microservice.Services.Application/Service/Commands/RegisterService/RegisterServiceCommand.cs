namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RegisterService;

/// <summary>
/// Lo que un microservicio envia al arrancar: su identidad y todos sus controllers con sus acciones.
/// </summary>
public record RegisterServiceCommand(Guid Id, string Name, string Description, List<ControllerDto> Controllers) : IRequest;

public class Validator : AbstractValidator<RegisterServiceCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(512);
    }
}
