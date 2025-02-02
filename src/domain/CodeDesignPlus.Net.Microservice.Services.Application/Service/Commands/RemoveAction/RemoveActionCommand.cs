namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RemoveAction;

[DtoGenerator]
public record RemoveActionCommand(Guid IdService, Guid IdController, Guid IdAction) : IRequest;

public class Validator : AbstractValidator<RemoveActionCommand>
{
    public Validator()
    {
        RuleFor(x => x.IdService).NotEmpty().NotNull();
        RuleFor(x => x.IdController).NotEmpty().NotNull();
        RuleFor(x => x.IdAction).NotEmpty().NotNull();
    }
}
