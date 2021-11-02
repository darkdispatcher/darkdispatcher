using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Domain.Projects
{
  public record ConfigurationId(ProjectId ProjectId, string Value) : AggregateId(ProjectId.TenantId, Value);
  
  public sealed class Configuration : Aggregate<ConfigurationState, ConfigurationId>
  {
    public Configuration(ConfigurationId id, string name, string? description = null)
    {
      var @event = new ProjectEvents.V1.ConfigurationCreated(id, name, description);
      Apply(@event);
    }

    public void Update(string name, string description)
    {
      var @event = new ProjectEvents.V1.ConfigurationUpdated(GetAggregateId(), name, description);
      Apply(@event);
    }
  }

  public record ConfigurationState : AggregateState<ConfigurationState, ConfigurationId>
  {
    public ConfigurationState()
    {
      On<ProjectEvents.V1.ConfigurationCreated>((state, created) => state with
      {
        Id = created.ConfigurationId,
        Name = created.Name,
        Description = created.Description
      });
      
      On<ProjectEvents.V1.ConfigurationUpdated>((state, updated) => state with
      {
        Name = updated.Name,
        Description = updated.Description
      });
      
      On<ProjectEvents.V1.ConfigurationDeleted>((state, deleted) => state with
      {
        IsDeleted = true
      });
    }
    
    public string Name { get; init; } = null!;
    
    public string? Description { get; init; }

    public bool IsDeleted { get; init; }
  }
}