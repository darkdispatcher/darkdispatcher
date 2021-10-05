using System.Collections.Generic;
using System.Linq;
using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Projects;
using Environment = DarkDispatcher.Domain.Projects.Environment;

namespace DarkDispatcher.Application.Features.Projects.Projections
{
  public class ProjectProjection : 
    IProjection<ProjectEvents.V1.ProjectCreated>,
    IProjection<ProjectEvents.V1.ProjectDeleted>,
    IProjection<ProjectEvents.V1.ProjectUpdated>,
    IProjection<ProjectEvents.V1.EnvironmentCreated>,
    IProjection<ProjectEvents.V1.EnvironmentUpdated>,
    IProjection<ProjectEvents.V1.EnvironmentDeleted>
  {
    public string OrganizationId { get; private set; } = null!;
    public string Id { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsDeleted { get; private set; }
    public List<Environment> Environments { get; private set; }
  
    public void Apply(ProjectEvents.V1.ProjectCreated @event)
    {
      OrganizationId = @event.ProjectId.TenantId!;
      Id =  @event.ProjectId.Value;
      Name = @event.Name;
      Description = @event.Description;
      Environments = new List<Environment>();
    }
  
    public void Apply(ProjectEvents.V1.ProjectDeleted @event)
    {
      IsDeleted = true;
    }
  
    public void Apply(ProjectEvents.V1.ProjectUpdated @event)
    {
      Name = @event.Name;
      Description = @event.Description;
    }
  
    public void Apply(ProjectEvents.V1.EnvironmentCreated @event)
    {
      var environment = new Environment(@event.Id, @event.Name, @event.Description, EnvironmentColor.FindColorOrDefault(@event.Color));
      Environments.Add(environment);
    }

    public void Apply(ProjectEvents.V1.EnvironmentUpdated @event)
    {
      var index = Environments.FindIndex(x => x.Id == @event.Id);
      if (index >= 0)
      {
        Environments[index] = Environments[index] with
        {
          Name = @event.Name,
          Description = @event.Description,
          Color = EnvironmentColor.FindColorOrDefault(@event.Color)
        };
      }
    }

    public void Apply(ProjectEvents.V1.EnvironmentDeleted @event)
    {
      var environment = Environments.SingleOrDefault(x => x.Id == @event.Id);

      if (environment != null)
        Environments.Remove(environment);
    }
  }
}