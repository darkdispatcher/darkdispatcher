using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DarkDispatcher.Application.Modules.Accounts.Queries;
using DarkDispatcher.Infrastructure.Extensions;
using DarkDispatcher.Server.Admin.v1.Organizations.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DarkDispatcher.Server.Admin.v1.Organizations;

[ApiController]
[Route("v1/organizations")]
public class ListOrganizations : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly ILogger<ListOrganizations> _logger;

  public ListOrganizations(
    IMediator mediator,
    ILogger<ListOrganizations> logger)
  {
    _mediator = mediator;
    _logger = logger;
  }

  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IEnumerable<Organization>), 200)]
  public async Task<IActionResult> Get()
  {
    try
    {
      var userId = User.GetUserId();
      var organizations = await _mediator.Send(new GetOrganizationsByUser.Query(userId));

      var results = organizations.Select(x => new Organization
      {
        Id = x.Id,
        Name = x.Name
      });

      return Ok(results);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error getting organizations.");
      return new StatusCodeResult(500);
    }
  }
}
