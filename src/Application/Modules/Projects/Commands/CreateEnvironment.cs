using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Commands;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Projects;
using DarkDispatcher.Domain.Projects.Entities;
using DarkDispatcher.Domain.Projects.Ids;
using FluentValidation;

namespace DarkDispatcher.Application.Modules.Projects.Commands;

public class CreateEnvironment
{
  public record Command(ProjectId ProjectId, string Name, string Description, EnvironmentColor Color) : ICommand<Environment>;

  internal class Validator : AbstractValidator<Command>
  {
    public Validator()
    {
      const int min = 4;
      const int max = 50;

      RuleFor(x => x.ProjectId)
        .NotEmpty().WithMessage("ProjectId is required.");

      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .Length(min, max).WithMessage($"Name must be between {min} and {max} characters.");

      RuleFor(x => x.Description)
        .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");
    }
  }

  internal class Handler : ICommandHandler<Command, Environment>
  {
    private readonly IAggregateStore _store;
    private readonly IIdGenerator _idGenerator;

    public Handler(IAggregateStore store, IIdGenerator idGenerator)
    {
      _store = store;
      _idGenerator = idGenerator;
    }

    public async Task<Environment> Handle(Command request, CancellationToken cancellationToken)
    {
      var id = _idGenerator.New();
      var environmentId = new EnvironmentId(request.ProjectId, id);
      var environment = new Environment(environmentId, request.Name, request.Description, request.Color);
      var created = await _store.StoreAsync(environment, cancellationToken);

      return created;
    }
  }
}
