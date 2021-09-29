using System.Collections.Generic;
using DarkDispatcher.Core.Events;
using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Projects.Events.v1;
using Environment = DarkDispatcher.Domain.Projects.Environment;

namespace DarkDispatcher.Application.Features.Projects.Projections
{
  public class ProjectProjection : IProjection
  {
    public string OrganizationId { get; private set; }
    public string Id { get; private set; }

    public string Name { get; private set; }
    public string Description { get; private set; }

    public bool IsDeleted { get; private set; }
    public ICollection<Environment> Environments { get; private set; }

    public void When(IDomainEvent @event)
    {
      switch (@event)
      {
        case ProjectCreated created:
          OrganizationId = created.TenantId;
          Id = created.Id;
          Name = created.Name;
          Description = created.Description;
          Environments = new List<Environment>();
          break;
        case ProjectDeleted:
          IsDeleted = true;
          break;
        case ProjectUpdated updated:
          Name = updated.Name;
          Description = updated.Description;
          break;
        case EnvironmentAdded added:
          Environments.Add(new Environment(added.EnvironmentName, added.EnvironmentDescription));
          break;
      }
    }
  }
}