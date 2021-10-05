using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Projects;

namespace DarkDispatcher.Domain.Features
{
  public record FeatureId(ConfigurationId ConfigurationId, string Value) : AggregateId(ConfigurationId.TenantId, Value);

  public sealed class Feature : Aggregate<FeatureState, FeatureId>
  {
    public Feature(FeatureId id, string key, string name, string? description = null)
    {
      var @event = new FeatureEvents.V1.FeatureCreated(id, key, name, description);
      Apply(@event);
    }
  }

  public record FeatureState : AggregateState<FeatureState, FeatureId>
  {
    public FeatureState()
    {
      On<FeatureEvents.V1.FeatureCreated>((state, created) => state with
      {
        Id = created.FeatureId,
        Key = created.Key,
        Name = created.Name,
        Description = created.Description
      });
      
      On<FeatureEvents.V1.FeatureUpdated>((state, updated) => state with
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