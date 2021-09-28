using DarkDispatcher.Core.Domain;
using DarkDispatcher.Domain.Accounts.Events.v1;

namespace DarkDispatcher.Application.Features.Accounts.Projections
{
  public record OrganizationProjection : IProjection
  {
    public string Id { get; private set; }
    
    public string Name { get; private set; }

    public bool IsDeleted { get; private set; }

    public void Apply(IDomainEvent @event)
    {
      switch (@event)
      {
        case OrganizationCreated(var id, var name):
          Id = id;
          Name = name;
          break;
        case OrganizationDeleted:
          IsDeleted = true;
          break;
        case OrganizationUpdated updated:
          Name = updated.Name;
          break;
      }
    }
  }
}