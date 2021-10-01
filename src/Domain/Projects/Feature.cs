using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Projects.Events.v1;

namespace DarkDispatcher.Domain.Projects
{
  public record FeatureId(ConfigurationId ConfigurationId, string Value) : AggregateId(ConfigurationId.TenantId, Value);

  public sealed class Feature : Aggregate<FeatureState, FeatureId>
  {
    public Feature(FeatureId id, string key, string name, string? description = null)
    {
      var @event = new FeatureCreated(id, key, name, description);
      Apply(@event);
    }
  }

  public record FeatureState : AggregateState<FeatureState, FeatureId>
  {
    public FeatureState()
    {
      On<FeatureCreated>((state, created) => state with
      {
        Id = created.FeatureId,
        Key = created.Key,
        Name = created.Name,
        Description = created.Description
      });
      
      On<FeatureUpdated>((state, updated) => state with
      {
        Key = updated.Key,
        Name = updated.Name,
        Description = updated.Description
      });
    }

    public string Key { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
  }
}