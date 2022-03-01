using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Projects.Events.v1;

namespace DarkDispatcher.Application.Modules.Projects.Projections;

public class ProjectProjection :
  IProjection<ProjectCreated>,
  IProjection<ProjectDeleted>,
  IProjection<ProjectUpdated>
{
  public string OrganizationId { get; private set; } = null!;
  public string Id { get; private set; } = null!;
  public string Name { get; private set; } = null!;
  public string? Description { get; private set; }
  public bool IsDeleted { get; private set; }

  public void Apply(ProjectCreated @event)
  {
    OrganizationId = @event.ProjectId.TenantId!;
    Id = @event.ProjectId.Value;
    Name = @event.Name;
    Description = @event.Description;
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
}
