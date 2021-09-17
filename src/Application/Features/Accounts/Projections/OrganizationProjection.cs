using DarkDispatcher.Core.Domain;
using DarkDispatcher.Domain.Accounts.Events.v1;

namespace DarkDispatcher.Application.Features.Accounts.Projections
{
  public record OrganizationProjection : IProjection
  {
    public string Id { get; set; }
    
    public string Name { get; set; }

    public bool IsDeleted { get; set; }

    public void Apply(IDomainEvent @event)
    {
      switch (@event)
      {
        case OrganizationCreated created:
          Id = created.Id;
          Name = created.Name;
          break;
        case OrganizationDeleted deleted:
          IsDeleted = true;
          break;
        case OrganizationUpdated updated:
          Name = updated.Name;
          break;
      }
    }
  }
}