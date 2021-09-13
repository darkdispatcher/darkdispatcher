using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core;
using DarkDispatcher.Core.Domain;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Accounts;
using MediatR;

namespace DarkDispatcher.Application.Features.Accounts.Queries
{
  public class GetOrganization
  {
    public record Query(string OrganizationId) : IQuery<Organization>;
    
    internal class Handler : IRequestHandler<Query, Organization>
    {
      private readonly IAggregateStore _store;

      public Handler(IAggregateStore store)
      {
        _store = store;
      }

      public async Task<Organization> Handle(Query request, CancellationToken cancellationToken)
      {
        var organization = await _store.LoadAsync<Organization>(request.OrganizationId, cancellationToken: cancellationToken);

        return organization;
      }
    }
  }
}