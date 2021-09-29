using System.Collections.Generic;
using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Projects.Events.v1;
using Environment = DarkDispatcher.Domain.Projects.Environment;

namespace DarkDispatcher.Application.Features.Projects.Projections
{
  public class ProjectProjection : 
    IProjection<ProjectCreated>,
    IProjection<ProjectDeleted>,
    IProjection<ProjectUpdated>,
    IProjection<EnvironmentAdded>
  {
    public string OrganizationId { get; private set; }
    public string Id { get; private set; }
  
    public string Name { get; private set; }
    public string Description { get; private set; }
  
    public bool IsDeleted { get; private set; }
    public ICollection<Environment> Environments { get; private set; }
  
    public void Apply(ProjectCreated @event)
    {
      OrganizationId = @event.TenantId;
      Id = @event.Id;
      Name = @event.Name;
      Description = @event.Description;
      Environments = new List<Environment>();
    }
  
    public void Apply(ProjectDeleted @event)
    {
      IsDeleted = true;
    }
  
    public void Apply(ProjectUpdated @event)
    {
      Name = @event.Name;
      Description = @event.Description;
    }
  
    public void Apply(EnvironmentAdded @event)
    {
      var environment = new Environment(@event.EnvironmentName, @event.EnvironmentDescription);
      Environments.Add(environment);
    }
  }
}