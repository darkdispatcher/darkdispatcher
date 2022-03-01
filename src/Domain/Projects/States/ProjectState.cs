using System.Collections.Generic;
using System.Linq;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Projects.Entities;
using DarkDispatcher.Domain.Projects.Events.v1;
using DarkDispatcher.Domain.Projects.Ids;

namespace DarkDispatcher.Domain.Projects.States;

public record ProjectState : AggregateState<ProjectState, ProjectId>
{
  public ProjectState()
  {
    RegisterProjectEventHandlers();
    RegisterTagEventHandlers();
  }

  public string Name { get; init; } = null!;
  public string? Description { get; init; }
  public bool IsDeleted { get; init; }
  public List<Tag> Tags { get; } = new();

  private void RegisterProjectEventHandlers()
  {
    On<ProjectCreated>((state, created) => state with
    {
      Id = created.ProjectId,
      Name = created.Name,
      Description = created.Description
    });

    On<ProjectUpdated>((state, updated) => state with
    {
      Name = updated.Name,
      Description = updated.Description
    });

    On<ProjectDeleted>((state, _) => state with
    {
      IsDeleted = true
    });
  }

  private void RegisterTagEventHandlers()
  {
    On<TagCreated>((state, added) =>
    {
      var color = TagColor.FindColorOrDefault(added.Name);
      var tag = new Tag(added.Id, added.Name, color);
      state.Tags.Add(tag);
      return state;
    });

    On<TagUpdated>((state, updated) =>
    {
      var index = state.Tags.FindIndex(x => x.Id == updated.Id);
      if (index >= 0)
      {
        state.Tags[index] = Tags[index] with
        {
          Name = updated.Name,
          Color = TagColor.FindColorOrDefault(updated.Color)
        };
      }

      return state;
    });

    On<TagDeleted>((state, deleted) =>
    {
      var tag = state.Tags.SingleOrDefault(x => x.Id == deleted.Id);

      if (tag != null)
        state.Tags.Remove(tag);

      return state;
    });
  }
}
