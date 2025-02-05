namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.CreateService;

[DtoGenerator]
public record CreateServiceCommand(Guid Id, string Name, string Description) : IRequest;

public class Validator : AbstractValidator<CreateServiceCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(512);
    }
}
