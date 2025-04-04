using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateAction;

[DtoGenerator]
public record UpdateActionCommand(Guid IdService, Guid IdController, Guid IdAction, string Name, string Description, Domain.Enums.HttpMethod HttpMethod) : IRequest;

public class Validator : AbstractValidator<UpdateActionCommand>
{
    public Validator()
    {
        RuleFor(x => x.IdService).NotEmpty().NotNull();
        RuleFor(x => x.IdController).NotEmpty().NotNull();
        RuleFor(x => x.IdAction).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(512);
    }
}
