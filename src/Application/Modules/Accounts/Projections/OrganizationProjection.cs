using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Accounts.Events.v1;

namespace DarkDispatcher.Application.Modules.Accounts.Projections;

public record OrganizationProjection :
  IProjection<OrganizationCreated>,
  IProjection<OrganizationDeleted>,
  IProjection<OrganizationUpdated>
{
  public string Id { get; set; } = null!;

  public string Name { get; set; } = null!;

  public bool IsDeleted { get; set; }

  public void Apply(OrganizationCreated @event)
  {
    Id = @event.Id;
    Name = @event.Name;
  }

  public void Apply(OrganizationDeleted @event)
  {
    IsDeleted = true;
  }

  public void Apply(OrganizationUpdated @event)
  {
    Name = @event.Name;
  }
}
