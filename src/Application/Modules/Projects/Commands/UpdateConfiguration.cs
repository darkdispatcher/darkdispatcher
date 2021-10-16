using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Commands;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Projects;
using FluentValidation;

namespace DarkDispatcher.Application.Features.Projects.Commands
{
  public class UpdateConfiguration
  {
    public record Command(Configuration Configuration) : ICommand<Configuration>;

    internal class Validator : AbstractValidator<Command>
    {
      public Validator(IReadRepository repository)
      {
        // TODO: Add Validation
      }
    }
    
    internal class Handler : ICommandHandler<Command, Configuration>
    {
      private readonly IAggregateStore _store;

      public Handler(IAggregateStore store)
      {
        _store = store;
      }
      
      public async Task<Configuration> Handle(Command request, CancellationToken cancellationToken)
      {
        var updated = await _store.StoreAsync(request.Configuration, cancellationToken);
        return updated;
      }
    }
  }
}