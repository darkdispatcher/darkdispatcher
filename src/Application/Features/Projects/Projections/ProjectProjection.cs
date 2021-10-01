using System.Collections.Generic;
using System.Linq;
using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Projects;
using DarkDispatcher.Domain.Projects.Events.v1;
using Environment = DarkDispatcher.Domain.Projects.Environment;

namespace DarkDispatcher.Application.Features.Projects.Projections
{
  public class ProjectProjection : 
    IProjection<ProjectCreated>,
    IProjection<ProjectDeleted>,
    IProjection<ProjectUpdated>,
    IProjection<EnvironmentCreated>,
    IProjection<EnvironmentUpdated>,
    IProjection<EnvironmentDeleted>
  {
    public string OrganizationId { get; private set; } = null!;
    public string Id { get; private set; }= null!;
    public string Name { get; private set; }= null!;
    public string? Description { get; private set; }
    public bool IsDeleted { get; private set; }
    public List<Environment> Environments { get; private set; }
    public List<ConfigurationState> Configurations { get; private set; }
  
    public void Apply(ProjectCreated @event)
    {
      OrganizationId = @event.ProjectId.TenantId!;
      Id =  @event.ProjectId.Value;
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
  
    public void Apply(EnvironmentCreated @event)
    {
      var environment = new Environment(@event.Id, @event.Name, @event.Description, EnvironmentColor.FindColorOrDefault(@event.Color));
      Environments.Add(environment);
    }

    public void Apply(EnvironmentUpdated @event)
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

    public void Apply(EnvironmentDeleted @event)
    {
      var environment = Environments.SingleOrDefault(x => x.Id == @event.Id);

      if (environment != null)
        Environments.Remove(environment);
    }
  }
}