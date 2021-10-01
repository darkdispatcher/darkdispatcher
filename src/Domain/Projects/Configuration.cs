using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Projects.Events.v1;

namespace DarkDispatcher.Domain.Projects
{
  public record ConfigurationId(ProjectId ProjectId, string Value) : AggregateId(ProjectId.TenantId, Value);
  
  public sealed class Configuration : Aggregate<ConfigurationState, ConfigurationId>
  {
    public Configuration(ConfigurationId id, string name, string description)
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

  public record ConfigurationState : AggregateState<ConfigurationState, ConfigurationId>
  {
    public ConfigurationState()
    {
      On<ConfigurationCreated>((state, created) => state with
      {
        Id = created.ConfigurationId,
        Name = created.Name,
        Description = created.Description
      });
      
      On<ConfigurationUpdated>((state, updated) => state with
      {
        Name = updated.Name,
        Description = updated.Description
      });
    }
    
    public string Name { get; init; } = null!;
    
    public string? Description { get; init; }
  }
}