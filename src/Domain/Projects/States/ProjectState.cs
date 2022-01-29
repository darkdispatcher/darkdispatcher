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
    RegisterEnvironmentEventHandlers();
    RegisterTagEventHandlers();
  }

  public string Name { get; init; } = null!;
  public string? Description { get; init; }
  public bool IsDeleted { get; init; }
  public List<Environment> Environments { get; } = new();
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

  private void RegisterEnvironmentEventHandlers()
  {
    On<EnvironmentCreated>((state, added) =>
    {
      var color = EnvironmentColor.FindColorOrDefault(added.Name);
      var environment = new Environment(added.Id, added.Name, added.Description, color);
      state.Environments.Add(environment);
      return state;
    });

    On<EnvironmentUpdated>((state, updated) =>
    {
      var index = state.Environments.FindIndex(x => x.Id == updated.Id);
      if (index >= 0)
      {
        state.Environments[index] = Environments[index] with
        {
          Name = updated.Name,
          Description = updated.Description,
          Color = EnvironmentColor.FindColorOrDefault(updated.Color)
        };
      }

      return state;
    });

    On<EnvironmentDeleted>((state, deleted) =>
    {
      var environment = state.Environments.SingleOrDefault(x => x.Id == deleted.Id);

      if (environment != null)
        state.Environments.Remove(environment);

      return state;
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
