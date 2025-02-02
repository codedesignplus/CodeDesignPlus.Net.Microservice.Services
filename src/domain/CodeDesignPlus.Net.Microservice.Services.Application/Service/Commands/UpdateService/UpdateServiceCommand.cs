namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;

[DtoGenerator]
public record UpdateServiceCommand(Guid Id, string Name, string Description, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateServiceCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull();
        RuleFor(x => x.Description).NotEmpty().NotNull();
    }
}
