namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.DeleteService;

[DtoGenerator]
public record DeleteServiceCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteServiceCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
