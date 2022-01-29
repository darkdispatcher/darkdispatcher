using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Commands;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Projects;

namespace DarkDispatcher.Application.Modules.Projects.Commands;

public class UpdateProject
{
  public record Command(Project Project) : ICommand<Project>;

  internal class Handler : ICommandHandler<Command, Project>
  {
    private readonly IAggregateStore _store;

    public Handler(IAggregateStore store)
    {
      _store = store;
    }
      
    public async Task<Project> Handle(Command request, CancellationToken cancellationToken)
    {
      var updated = await _store.StoreAsync(request.Project, cancellationToken);
      return updated;
    }
  }
}