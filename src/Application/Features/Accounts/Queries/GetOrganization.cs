using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Application.Features.Accounts.Projections;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Core.Queries;
using DarkDispatcher.Domain.Accounts;
using Organization = DarkDispatcher.Application.Features.Accounts.Projections.Organization;

namespace DarkDispatcher.Application.Features.Accounts.Queries
{
  public class GetOrganization
  {
    public record Query(OrganizationId OrganizationId) : IQuery<Organization>;
    
    internal class Handler : IQueryHandler<Query, Organization>
    {
      private readonly IReadRepository _repository;

      public Handler(IReadRepository repository)
      {
        _repository = repository;
      }

      public async Task<Organization> Handle(Query request, CancellationToken cancellationToken)
      {
        var organization = await _repository.FindAsync<Organization, OrganizationId>(request.OrganizationId, cancellationToken);

        return organization;
      }
    }
  }
}