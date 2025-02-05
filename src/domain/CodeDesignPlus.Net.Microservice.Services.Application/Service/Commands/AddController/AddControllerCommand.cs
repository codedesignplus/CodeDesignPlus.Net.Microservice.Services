namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddController;

[DtoGenerator]
public record AddControllerCommand(Guid IdService, Guid IdController, string Name, string Description) : IRequest;

public class Validator : AbstractValidator<AddControllerCommand>
{
    public Validator()
    {
        RuleFor(x => x.IdService).NotEmpty().NotNull();
        RuleFor(x => x.IdController).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(512);
    }
}
