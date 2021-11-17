using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DarkDispatcher.Application.Modules.Accounts.Commands;
using DarkDispatcher.Application.Modules.Accounts.Projections;
using DarkDispatcher.Application.Modules.Accounts.Queries;
using DarkDispatcher.Grpc.V1;
using DarkDispatcher.Proto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DarkDispatcher.Infrastructure.Grpc.Services;

public class OrganizationService : DarkDispatcher.Grpc.V1.OrganizationService.OrganizationServiceBase
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

  public override async Task<ListOrganizationsResponse> ListOrganizations(Empty empty, ServerCallContext context)
  {
    var userId = GetUserId();
    var organizations = await _mediator.Send(new GetOrganizationsByUser.Query(userId), context.CancellationToken);
    var response = MapToOrganizationsResponse(organizations);

    return response;
  }

  public override async Task<Organization> GetOrganization(GetOrganizationRequest request, ServerCallContext context)
  {
    // TODO: verify data is owned by User
    var userId = GetUserId();

    var organization =
      await _mediator.Send(new GetOrganization.Query(request.OrganizationId), context.CancellationToken);
    var response = new Organization
    {
      OrganizationId = organization.Id,
      Name = organization.Name
    };

    return response;
  }

  public override async Task<Organization> CreateOrganization(CreateOrganizationRequest request, ServerCallContext context)
  {
    var userId = GetUserId();
    var organization = await _mediator.Send(new CreateOrganization.Command(request.Name), context.CancellationToken);

    return new Organization
    {
      OrganizationId = organization.State.Id.Value,
      Name = organization.State.Name
    };
  }

  public override async Task<Organization> UpdateOrganization(UpdateOrganizationRequest request,
    ServerCallContext context)
  {
    var userId = GetUserId();

    var organization = await _mediator.Send(new GetOrganization.Query(request.Organization.OrganizationId));

    if (organization is null)
      throw new RpcException(new Status(StatusCode.InvalidArgument, "Organization does not exist."));

    //await _mediator.Publish(new UpdateOrganization.Command())

    return new Organization
    {
      OrganizationId = organization.Id,
      Name = organization.Name
    };
  }

  // public override Task<UpdateOrganizationResponse> DeleteOrganization(DeleteOrganizationRequest request, ServerCallContext context)
  // {
  //   return base.DeleteOrganization(request, context);
  // }

  private static ListOrganizationsResponse MapToOrganizationsResponse(IEnumerable<OrganizationProjection> organizations)
  {
    var response = new ListOrganizationsResponse();
    var protoOrganizations = organizations.Select(x => new Organization
    {
      OrganizationId = x.Id,
      Name = x.Name
    });

    response.Organizations.AddRange(protoOrganizations);

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