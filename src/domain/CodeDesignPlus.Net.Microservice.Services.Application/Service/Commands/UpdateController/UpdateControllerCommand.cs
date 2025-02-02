namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateController;

[DtoGenerator]
public record UpdateControllerCommand(Guid IdService, Guid IdController, string Name, string Description) : IRequest;

public class Validator : AbstractValidator<UpdateControllerCommand>
{
    public Validator()
    {
        RuleFor(x => x.IdService).NotEmpty().NotNull();
        RuleFor(x => x.IdController).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull();
        RuleFor(x => x.Description).NotEmpty().NotNull();
    }
}
