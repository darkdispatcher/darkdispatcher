using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DarkDispatcher.Application.Features.Accounts.Queries;
using HotChocolate;
using HotChocolate.Types;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DarkDispatcher.Infrastructure.GraphQL.Organizations
{
  [ExtendObjectType(OperationTypeNames.Query)]
  public class OrganizationQueries
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrganizationQueries(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<IEnumerable<Organization>> Organizations([Service] IMediator mediator)
    {
      // TODO: Move to common method for all queries
      var user = _httpContextAccessor.HttpContext?.User;
      var userId = user?.Identity?.Name;

      var organizations = await mediator.Send(new GetOrganizationsByUser.Query(userId!));
      return organizations.Select(x => new Organization
      {
        Name = x.Name
      });
    }
  }
}