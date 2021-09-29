using DarkDispatcher.Core.Events;
using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Accounts.Events.v1;

namespace DarkDispatcher.Application.Features.Accounts.Projections
{
  public record Organization : IProjection
  {
    public string Id { get; private set; }
  
    public string Name { get; private set; }

    public bool IsDeleted { get; private set; }

    public void When(IDomainEvent @event)
    {
      switch (@event)
      {
        case OrganizationCreated created:
          Apply(created);
          break;
        case OrganizationDeleted deleted:
          IsDeleted = true;
          break;
        case OrganizationUpdated updated:
          Apply(updated);
          break;
      }
    }

    private void Apply(OrganizationCreated created)
    {
      Id = created.Id;
      Name = created.Name;
    }

    private void Apply(OrganizationDeleted deleted)
    {
      IsDeleted = true;
    }

    private void Apply(OrganizationUpdated updated)
    {
      Name = updated.Name;
    }
  }
}