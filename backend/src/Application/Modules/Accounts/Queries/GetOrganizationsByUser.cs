using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Application.Features.Accounts.Projections;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Core.Queries;
using DarkDispatcher.Domain.Accounts;

namespace DarkDispatcher.Application.Features.Accounts.Queries
{
  public class GetOrganizationsByUser
  {
    public record Query(string UserId) : IQuery<IReadOnlyCollection<OrganizationProjection>>;
    
    internal class Handler : IQueryHandler<Query, IReadOnlyCollection<OrganizationProjection>>
    {
      private readonly IReadRepository _repository;

      public Handler(IReadRepository repository)
      {
        _repository = repository;
      }
      
      public async Task<IReadOnlyCollection<OrganizationProjection>> Handle(Query request, CancellationToken cancellationToken)
      {
        // TODO: Use actual UserId
        var organizations =
          await _repository.ListAsync<OrganizationProjection, OrganizationId>(new OrganizationId(""), x => x.IsDeleted == false, cancellationToken);

        return organizations;
      }
    }
  }
}