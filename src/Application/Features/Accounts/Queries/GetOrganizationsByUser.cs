using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core;
using DarkDispatcher.Core.Domain;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Accounts;
using MediatR;

namespace DarkDispatcher.Application.Features.Accounts.Queries
{
  public class GetOrganizationsByUser
  {
    public record Query(string UserId) : IQuery<IReadOnlyCollection<Organization>>;
    
    internal class Handler : IRequestHandler<Query, IReadOnlyCollection<Organization>>
    {
      private readonly IReadRepository _repository;

      public Handler(IReadRepository repository)
      {
        _repository = repository;
      }
      
      public async Task<IReadOnlyCollection<Organization>> Handle(Query request, CancellationToken cancellationToken)
      {
        var organizations =
          await _repository.ListAsync<Organization>("", x => x.State.IsDeleted == false, cancellationToken);

        return organizations;
      }
    }
  }
}