using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Accounts;

namespace DarkDispatcher.Application.Features.Accounts.Projections
{
  public record OrganizationProjection : 
    IProjection<AccountEvents.V1.OrganizationCreated>,
    IProjection<AccountEvents.V1.OrganizationDeleted>,
    IProjection<AccountEvents.V1.OrganizationUpdated>
  {
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public void Apply(AccountEvents.V1.OrganizationCreated @event)
    {
      Id = @event.Id;
      Name = @event.Name;
    }

    public void Apply(AccountEvents.V1.OrganizationDeleted @event)
    {
      IsDeleted = true;
    }

    public void Apply(AccountEvents.V1.OrganizationUpdated @event)
    {
      Name = @event.Name;
    }
  }
}