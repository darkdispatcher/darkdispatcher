using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Commands;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Accounts;

namespace DarkDispatcher.Application.Features.Accounts.Commands
{
  public class UpdateOrganization
  {
    public record Command(Organization Organization) : ICommand<Organization>;

    internal class Handler : ICommandHandler<Command, Organization>
    {
      private readonly IAggregateStore _store;

      public Handler(IAggregateStore store)
      {
        _store = store;
      }
      
      public async Task<Organization> Handle(Command request, CancellationToken cancellationToken)
      {
        var updated = await _store.StoreAsync(request.Organization, cancellationToken);
        return updated;
      }
    }
  }
}