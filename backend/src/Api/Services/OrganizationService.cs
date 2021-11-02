using System.Linq;
using System.Threading.Tasks;
using DarkDispatcher.Application.Features.Accounts.Queries;
using Darkdispatcher.Grpc.Organizations.V1;
using Darkdispatcher.Proto;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DarkDispatcher.Api.Services
{
  public class OrganizationService : Darkdispatcher.Grpc.Organizations.V1.OrganizationService.OrganizationServiceBase
  {
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<OrganizationService> _logger;

    public OrganizationService(
      IMediator mediator,
      IHttpContextAccessor httpContextAccessor,
      ILogger<OrganizationService> logger)
    {
      _mediator = mediator;
      _httpContextAccessor = httpContextAccessor;
      _logger = logger;
    }

    public override async Task<GetOrganizationsResponse> GetOrganizations(
      GetOrganizationsRequest request,
      ServerCallContext context)
    {
      var response = new GetOrganizationsResponse();
      var userId = GetUserId();

      var organizations = await _mediator.Send(new GetOrganizationsByUser.Query(userId), context.CancellationToken);
      var protoOrganizations = organizations.Select(x => new Organization
      {
        Id = x.Id,
        Name = x.Name
      });

      response.Organizations.AddRange(protoOrganizations);

      return response;
    }
    
    public override async Task<Organization> GetOrganization(
      GetOrganizationRequest request,
      ServerCallContext context)
    {
      // TODO: verify data is owned by User
      var userId = GetUserId();

      var query = new GetOrganization.Query(request.Id);
      var organization = await _mediator.Send(query, context.CancellationToken);
      var response = new Organization
      {
        Id = organization.Id,
        Name = organization.Name
      };

      return response;
    }

    private string? GetUserId()
    {
      // TODO: Move to common method for all queries
      var user = _httpContextAccessor.HttpContext?.User;
      var userId = user?.Identity?.Name;

      return userId;
    }
  }
}