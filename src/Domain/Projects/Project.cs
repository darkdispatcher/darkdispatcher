using System.Collections.Generic;
using System.Linq;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Accounts;

namespace DarkDispatcher.Domain.Projects
{
  public record ProjectId(OrganizationId OrganizationId, string Value) : AggregateId(OrganizationId.Value, Value);

  public sealed class Project : Aggregate<ProjectState, ProjectId>
  {
    public Project(ProjectId id, string name, string description)
    {
      var @event = new ProjectEvents.V1.ProjectCreated(id, name, description);
      Apply(@event);
    }

    public void Update(string name, string description)
    {
      var @event = new ProjectEvents.V1.ProjectUpdated(GetAggregateId(), name, description);
      Apply(@event);
    }

    public void Delete(string name)
    {
      var @event = new ProjectEvents.V1.ProjectDeleted(GetAggregateId(), name);
      Apply(@event);
    }

    public void CreateEnvironment(Environment environment)
    {
      var (id, name, description, color) = environment;
      var @event = new ProjectEvents.V1.EnvironmentCreated(GetAggregateId(), id, name, description, color.ToString());
      Apply(@event);
    }

    public void UpdateEnvironment(Environment environment)
    {
      var (id, name, description, color) = environment;
      var @event = new ProjectEvents.V1.EnvironmentUpdated(GetAggregateId(), id, name, description, color.ToString());
      Apply(@event);
    }

    public void DeleteEnvironment(Environment environment)
    {
      var @event = new ProjectEvents.V1.EnvironmentDeleted(GetAggregateId(), environment.Id, environment.Name);
      Apply(@event);
    }

    public void CreateTag(Tag tag)
    {
      var (id, name, color) = tag;
      var @event = new ProjectEvents.V1.TagCreated(GetAggregateId(), id, name, color.ToString());
      Apply(@event);
    }

    public void UpdateTag(Tag tag)
    {
      var (id, name, color) = tag;
      var @event = new ProjectEvents.V1.TagUpdated(GetAggregateId(), id, name, color.ToString());
      Apply(@event);
    }

    public void DeleteTag(Tag tag)
    {
      var @event = new ProjectEvents.V1.TagDeleted(GetAggregateId(), tag.Id, tag.Name);
      Apply(@event);
    }
  }

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
      On<ProjectEvents.V1.ProjectCreated>((state, created) => state with
      {
        Id = created.ProjectId,
        Name = created.Name,
        Description = created.Description
      });

      On<ProjectEvents.V1.ProjectUpdated>((state, updated) => state with
      {
        Name = updated.Name,
        Description = updated.Description
      });

      On<ProjectEvents.V1.ProjectDeleted>((state, _) => state with
      {
        IsDeleted = true
      });
    }

    private void RegisterEnvironmentEventHandlers()
    {
      On<ProjectEvents.V1.EnvironmentCreated>((state, added) =>
      {
        var color = EnvironmentColor.FindColorOrDefault(added.Name);
        var environment = new Environment(added.Id, added.Name, added.Description, color);
        state.Environments.Add(environment);
        return state;
      });

      On<ProjectEvents.V1.EnvironmentUpdated>((state, updated) =>
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

      On<ProjectEvents.V1.EnvironmentDeleted>((state, deleted) =>
      {
        var environment = state.Environments.SingleOrDefault(x => x.Id == deleted.Id);

        if (environment != null)
          state.Environments.Remove(environment);

        return state;
      });
    }

    private void RegisterTagEventHandlers()
    {
      On<ProjectEvents.V1.TagCreated>((state, added) =>
      {
        var color = TagColor.FindColorOrDefault(added.Name);
        var tag = new Tag(added.Id, added.Name, color);
        state.Tags.Add(tag);
        return state;
      });

      On<ProjectEvents.V1.TagUpdated>((state, updated) =>
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

      On<ProjectEvents.V1.TagDeleted>((state, deleted) =>
      {
        var tag = state.Tags.SingleOrDefault(x => x.Id == deleted.Id);

        if (tag != null)
          state.Tags.Remove(tag);

        return state;
      });
    }
  }
}