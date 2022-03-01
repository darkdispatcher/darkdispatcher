using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Projects.Entities;
using DarkDispatcher.Domain.Projects.Events.v1;
using DarkDispatcher.Domain.Projects.Ids;
using DarkDispatcher.Domain.Projects.States;

namespace DarkDispatcher.Domain.Projects;

public sealed class Environment : Aggregate<EnvironmentState, EnvironmentId>
{
  public Environment(EnvironmentId id, string name, string description, EnvironmentColor color)
  {
    var @event = new EnvironmentCreated(id, name, description, color.ToString());
    Apply(@event);
  }

  public void Update(string name, string description, EnvironmentColor color)
  {
    var @event = new EnvironmentUpdated(GetAggregateId(), name, description, color.ToString());
    Apply(@event);
  }

  public void Delete(string name)
  {
    var @event = new EnvironmentDeleted(GetAggregateId(), name);
    Apply(@event);
  }
}
