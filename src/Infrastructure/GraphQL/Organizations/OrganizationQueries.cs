using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DarkDispatcher.Application.Features.Accounts.Queries;
using HotChocolate.Types;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DarkDispatcher.Infrastructure.GraphQL.Organizations
{
  [ExtendObjectType(OperationTypeNames.Query)]
  public class OrganizationQueries
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;

    public OrganizationQueries(
      IHttpContextAccessor httpContextAccessor,
      IMediator mediator)
    {
      _httpContextAccessor = httpContextAccessor;
      _mediator = mediator;
    }
    
    public async Task<IEnumerable<OrganizationType>> Organizations()
    {
      var user = _httpContextAccessor.HttpContext?.User;
      var userId = user?.Identity?.Name;

      var organizations = await  _mediator.Send(new GetOrganizationsByUser.Query(userId!));
      return organizations.Select(x => new OrganizationType
      {
        Name = "Yo"
      });
    }
  }
}