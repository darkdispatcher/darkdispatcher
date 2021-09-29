using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Accounts.Events.v1;

namespace DarkDispatcher.Application.Features.Accounts.Projections
{
  public record OrganizationProjection : 
    IProjection<OrganizationCreated>,
    IProjection<OrganizationDeleted>,
    IProjection<OrganizationUpdated>
  {
    public string Id { get; private set; }
  
    public string Name { get; private set; }

    public bool IsDeleted { get; private set; }

    public void Apply(OrganizationCreated created)
    {
      Id = created.Id;
      Name = created.Name;
    }

    public void Apply(OrganizationDeleted deleted)
    {
      IsDeleted = true;
    }

    public void Apply(OrganizationUpdated updated)
    {
      Name = updated.Name;
    }
  }
}