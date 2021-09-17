using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Accounts;
using MediatR;

namespace DarkDispatcher.Application.Features.Accounts.Commands
{
  public class CreateOrganization
  {
    public record Command(string Name) : IRequest<Organization>;
    
    internal class Handler : IRequestHandler<Command, Organization>
    {
      private readonly IAggregateStore _store;
      private readonly IIdGenerator _idGenerator;

      public Handler(IAggregateStore store, IIdGenerator idGenerator)
      {
        _store = store;
        _idGenerator = idGenerator;
      }
      
      public async Task<Organization> Handle(Command request, CancellationToken cancellationToken)
      {
        var id = _idGenerator.New();
        var organization = new Organization(new OrganizationId(id), request.Name);
        var created = await _store.StoreAsync(organization, cancellationToken);
        
        return created;
      }
    }
  }
}