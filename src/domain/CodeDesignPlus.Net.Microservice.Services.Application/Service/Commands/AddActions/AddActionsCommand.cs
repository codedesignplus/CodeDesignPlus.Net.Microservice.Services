using FluentValidation.Validators;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddActions;

[DtoGenerator]
public record AddActionsCommand(Guid IdService, Guid IdController, List<ActionDto> Actions) : IRequest;

public class Validator : AbstractValidator<AddActionsCommand>
{
    public Validator()
    {
        RuleFor(x => x.IdService).NotEmpty().NotNull();
        RuleFor(x => x.IdController).NotEmpty().NotNull();
        RuleFor(x => x.Actions).NotEmpty();
        RuleForEach(x => x.Actions).SetValidator(new ActionDtoValidator());
    }
}

public class ActionDtoValidator : AbstractValidator<ActionDto>
{
    public ActionDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(512);
        RuleFor(x => x.HttpMethod).NotEqual(Domain.Enums.HttpMethod.None);
    }
}