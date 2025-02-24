namespace CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceByName;

public record GetServiceByNameQuery(string Name) : IRequest<ServiceDto>;

public class Validator : AbstractValidator<GetServiceByNameQuery>
{
    public Validator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
    }
}

