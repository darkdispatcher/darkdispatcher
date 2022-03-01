using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Commands;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Accounts.Ids;
using DarkDispatcher.Domain.Projects;
using DarkDispatcher.Domain.Projects.Ids;
using FluentValidation;

namespace DarkDispatcher.Application.Modules.Projects.Commands;

public class CreateProject
{
  public record Command(OrganizationId OrganizationId, string Name, string Description) : ICommand<Project>;

  internal class Validator : AbstractValidator<Command>
  {
    public Validator()
    {
      const int min = 4;
      const int max = 50;

      RuleFor(x => x.OrganizationId)
        .NotEmpty().WithMessage("OrganizationId is required.");

      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .Length(min, max).WithMessage($"Name must be between {min} and {max} characters.");

      RuleFor(x => x.Description)
        .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");
    }
  }

  internal class Handler : ICommandHandler<Command, Project>
  {
    private readonly IAggregateStore _store;
    private readonly IIdGenerator _idGenerator;

    public Handler(IAggregateStore store, IIdGenerator idGenerator)
    {
      _store = store;
      _idGenerator = idGenerator;
    }

    public async Task<Project> Handle(Command request, CancellationToken cancellationToken)
    {
      var id = _idGenerator.New();
      var organizationId = request.OrganizationId.Value;
      var projectId = new ProjectId(organizationId, id);
      var project = new Project(projectId, request.Name, request.Description);
      var created = await _store.StoreAsync(project, cancellationToken);

      return created;
    }
  }
}
