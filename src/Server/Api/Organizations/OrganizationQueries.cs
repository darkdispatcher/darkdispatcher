using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DarkDispatcher.Application.Modules.Accounts.Queries;
using DarkDispatcher.Infrastructure.Extensions;
using HotChocolate;
using HotChocolate.Types;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DarkDispatcher.Server.Api.Organizations;

[ExtendObjectType(OperationTypeNames.Query)]
public class OrganizationQueries
{
  private readonly ILogger<OrganizationQueries> _logger;

  public OrganizationQueries(ILogger<OrganizationQueries> logger)
  {
    _logger = logger;
  }

  /// <summary>
  /// List of the Organizations that belongs to the user.
  /// </summary>
  /// <returns></returns>
  public async Task<IReadOnlyCollection<Organization>> Organizations(ClaimsPrincipal user, [Service] IMediator mediator)
  {
    try
    {
      var userId = user.GetUserId();

      var organizations = await mediator.Send(new GetOrganizationsByUser.Query(userId));
      var results = organizations.Select(x => new Organization
      {
        Id = x.Id,
        Name = x.Name
      }).ToImmutableList();
      return results;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error while getting organizations");
      return new List<Organization>();
    }
  }
}
