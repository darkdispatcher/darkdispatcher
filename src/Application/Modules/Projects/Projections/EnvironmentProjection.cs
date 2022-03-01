using DarkDispatcher.Core.Projections;
using DarkDispatcher.Domain.Projects.Events.v1;

namespace DarkDispatcher.Application.Modules.Projects.Projections;

public class EnvironmentProjection :
  IProjection<EnvironmentCreated>,
  IProjection<EnvironmentUpdated>,
  IProjection<EnvironmentDeleted>
{
  public string OrganizationId { get; private set; } = null!;
  public string ProjectId { get; private set; } = null!;
  public string Id { get; private set; } = null!;
  public string Name { get; private set; } = null!;
  public string? Description { get; private set; }
  public string Color { get; private set; }
  public bool IsDeleted { get; private set; }

  public void Apply(EnvironmentCreated @event)
  {
    OrganizationId = @event.EnvironmentId.ProjectId.TenantId;
    ProjectId = @event.EnvironmentId.ProjectId.Value;
    Id = @event.EnvironmentId.Value;
    Name = @event.Name;
    Description = @event.Description;
    Color = @event.Color;
  }

  public void Apply(EnvironmentUpdated @event)
  {
    Name = @event.Name;
    Description = @event.Description;
    Color = @event.Color;
  }

  public void Apply(EnvironmentDeleted @event)
  {
    IsDeleted = true;
  }
}
