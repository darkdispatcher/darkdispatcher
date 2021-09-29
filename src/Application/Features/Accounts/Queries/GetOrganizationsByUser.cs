using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Core.Queries;
using DarkDispatcher.Domain.Accounts;
using Organization = DarkDispatcher.Application.Features.Accounts.Projections.Organization;

namespace DarkDispatcher.Application.Features.Accounts.Queries
{
  public class GetOrganizationsByUser
  {
    public record Query(string UserId) : IQuery<IReadOnlyCollection<Organization>>;
    
    internal class Handler : IQueryHandler<Query, IReadOnlyCollection<Organization>>
    {
      private readonly IReadRepository _repository;

      public Handler(IReadRepository repository)
      {
        _repository = repository;
      }
      
      public async Task<IReadOnlyCollection<Organization>> Handle(Query request, CancellationToken cancellationToken)
      {
        var organizations =
          await _repository.ListAsync<Organization, OrganizationId>(new OrganizationId(""), x => x.IsDeleted == false, cancellationToken);

        return organizations;
      }
    }
  }
}