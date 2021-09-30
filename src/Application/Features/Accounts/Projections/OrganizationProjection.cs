using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Accounts.Events.v1;

namespace DarkDispatcher.Application.Features.Accounts.Projections
{
  public record OrganizationProjection : 
    IProjection<OrganizationCreated>,
    IProjection<OrganizationDeleted>,
    IProjection<OrganizationUpdated>
  {
    public string Id { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public bool IsDeleted { get; private set; }

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
}