using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Projects.Events.v1;
using DarkDispatcher.Domain.Projects.Ids;
using DarkDispatcher.Domain.Projects.States;

namespace DarkDispatcher.Domain.Projects;

public sealed class Configuration : Aggregate<ConfigurationState, ConfigurationId>
{
  public Configuration(ConfigurationId id, string name, string? description = null)
  {
    var @event = new ConfigurationCreated(id, name, description);
    Apply(@event);
  }

  public void Update(string name, string description)
  {
    var @event = new ConfigurationUpdated(GetAggregateId(), name, description);
    Apply(@event);
  }
}