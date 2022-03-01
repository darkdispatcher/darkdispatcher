using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Projects.Entities;
using DarkDispatcher.Domain.Projects.Events.v1;
using DarkDispatcher.Domain.Projects.Ids;
using DarkDispatcher.Domain.Projects.States;

namespace DarkDispatcher.Domain.Projects;

public sealed class Project : Aggregate<ProjectState, ProjectId>
{
  public Project(ProjectId id, string name, string description)
  {
    var @event = new ProjectCreated(id, name, description);
    Apply(@event);
  }

  public void Update(string name, string description)
  {
    var @event = new ProjectUpdated(GetAggregateId(), name, description);
    Apply(@event);
  }

  public void Delete(string name)
  {
    var @event = new ProjectDeleted(GetAggregateId(), name);
    Apply(@event);
  }

  public void CreateTag(Tag tag)
  {
    var (id, name, color) = tag;
    var @event = new TagCreated(GetAggregateId(), id, name, color.ToString());
    Apply(@event);
  }

  public void UpdateTag(Tag tag)
  {
    var (id, name, color) = tag;
    var @event = new TagUpdated(GetAggregateId(), id, name, color.ToString());
    Apply(@event);
  }

  public void DeleteTag(Tag tag)
  {
    var @event = new TagDeleted(GetAggregateId(), tag.Id, tag.Name);
    Apply(@event);
  }
}
