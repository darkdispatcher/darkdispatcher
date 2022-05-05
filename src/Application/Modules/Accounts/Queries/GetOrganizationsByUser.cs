using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Application.Modules.Accounts.Projections;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Core.Queries;
using DarkDispatcher.Domain.Accounts.Ids;

namespace DarkDispatcher.Application.Modules.Accounts.Queries;

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
      var organizationId = new OrganizationId("");
      var organizations =
        await _repository.ListAsync<OrganizationProjection, OrganizationId>(organizationId, x => x.IsDeleted == false && x.Users.Contains(request.UserId), cancellationToken);

      return organizations;
    }
  }
}
