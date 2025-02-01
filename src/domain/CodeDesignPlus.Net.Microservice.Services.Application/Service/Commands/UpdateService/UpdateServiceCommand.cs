namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;

[DtoGenerator]
public record UpdateServiceCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<UpdateServiceCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
