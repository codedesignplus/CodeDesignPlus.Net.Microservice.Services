namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.CreateService;

[DtoGenerator]
public record CreateServiceCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<CreateServiceCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
