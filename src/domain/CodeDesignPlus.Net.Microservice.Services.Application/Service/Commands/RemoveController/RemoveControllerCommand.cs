namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RemoveController;

[DtoGenerator]
public record RemoveControllerCommand(Guid IdService, Guid IdController) : IRequest;

public class Validator : AbstractValidator<RemoveControllerCommand>
{
    public Validator()
    {
        RuleFor(x => x.IdService).NotEmpty().NotNull();
        RuleFor(x => x.IdController).NotEmpty().NotNull();
    }
}
