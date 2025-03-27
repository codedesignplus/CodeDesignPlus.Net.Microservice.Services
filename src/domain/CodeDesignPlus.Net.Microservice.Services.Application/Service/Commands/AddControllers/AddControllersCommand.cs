namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddControllers;

[DtoGenerator]
public record AddControllersCommand(Guid IdService, List<ControllerDto> Controllers) : IRequest;

public class Validator : AbstractValidator<AddControllersCommand>
{
    public Validator()
    {
        RuleFor(x => x.IdService).NotEmpty().NotNull();
        RuleFor(x => x.Controllers).NotEmpty();
        RuleForEach(x => x.Controllers).SetValidator(new ControllerDtoValidator());
    }
}

public class ControllerDtoValidator : AbstractValidator<ControllerDto>
{
    public ControllerDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
    }
}