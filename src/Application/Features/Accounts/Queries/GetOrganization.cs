using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Application.Features.Accounts.Projections;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Core.Queries;
using DarkDispatcher.Domain.Accounts;

namespace DarkDispatcher.Application.Features.Accounts.Queries
{
  public class GetOrganization
  {
    public record Query(OrganizationId OrganizationId) : IQuery<OrganizationProjection>;
    
    internal class Handler : IQueryHandler<Query, OrganizationProjection>
    {
      private readonly IReadRepository _repository;

      public Handler(IReadRepository repository)
      {
        _repository = repository;
      }

      public async Task<OrganizationProjection> Handle(Query request, CancellationToken cancellationToken)
      {
        var organization = await _repository.FindAsync<OrganizationProjection, OrganizationId>(request.OrganizationId, cancellationToken);

        return organization;
      }
    }
  }
}