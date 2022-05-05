using System;
using System.Threading.Tasks;
using DarkDispatcher.Application.Modules.Accounts.Commands;
using DarkDispatcher.Domain.Accounts.Ids;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DarkDispatcher.Server.Api.Organizations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class OrganizationMutations
{
  private readonly ILogger<OrganizationMutations> _logger;

  public OrganizationMutations(ILogger<OrganizationMutations> logger)
  {
    _logger = logger;
  }

  [UseMutationConvention]
  public async Task<Organization?> UpdateOrganization([ID] string organizationId, string name, [Service] IMediator mediator)
  {
    try
    {
      var organization = await mediator.Send(new UpdateOrganization.Command(new OrganizationId(organizationId), name));
      return new Organization
      {
        Id = organization.GetId(),
        Name = organization.State.Name
      };
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error updating organization");
      return null;
    }
  }
}
