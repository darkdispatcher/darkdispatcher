using System;
using System.Linq;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Core.Extensions;
using DarkDispatcher.Domain.Features.Entities;
using DarkDispatcher.Domain.Features.Events.v1;
using DarkDispatcher.Domain.Features.Ids;
using DarkDispatcher.Domain.Features.States;
using DarkDispatcher.Domain.Projects.Entities;

namespace DarkDispatcher.Domain.Features;

public sealed class Feature : Aggregate<FeatureState, FeatureId>
{
  public Feature(
    FeatureId id,
    string key,
    string name,
    VariationType type,
    Variation[] variations,
    RuleVariationDefaults defaults,
    Tag[] tags,
    string? description = null)
  {
    if (State.Type is VariationType.String or VariationType.Number && variations.HasDuplicates(x => x.Value)) 
      throw new InvalidOperationException("All variation values must be unique.");

    var @event = new FeatureCreated(id, key, name, type, variations, defaults, tags, description);
    Apply(@event);
  }

  public void Update(string name, Tag[]? tags = null, string? description = null)
  {
    var @event = new FeatureUpdated(GetAggregateId(), name, tags, description);
    Apply(@event);
  }

  public void AddVariation(string id, string value, string? name = null, string? description = null)
  {
    if (State.Type == VariationType.Boolean)
      throw new InvalidOperationException($"Adding a variation for type '{nameof(VariationType.Boolean)}' is not allowed.");

    if (State.Variations.Any(x => x.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
      throw new InvalidOperationException("All variation values must be unique.");

    var @event = new VariationAdded(GetAggregateId(), id, value, name, description);
    Apply(@event);
  }

  public void UpdateVariation(string id, string value, string? name = null, string? description = null)
  {
    var values = new[] { "true", "false" };
    if (State.Type == VariationType.Boolean && !values.Any(x => x.Equals(value, StringComparison.InvariantCulture)))
      throw new InvalidOperationException($"Only '{string.Join(',', values)}' are accepted for '{nameof(VariationType.Boolean)}' variation values.");

    if (State.Variations.Any(x => x.Id != id && x.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
      throw new InvalidOperationException("All variation values must be unique.");

    var @event = new VariationUpdated(GetAggregateId(), id, value, name, description);
    Apply(@event);
  }

  public void DeleteVariation(string value)
  {
    if (State.Type == VariationType.Boolean)
      throw new InvalidOperationException(
        $"Deleting variations for type '{nameof(VariationType.Boolean)}' is not allowed.");

    var @event = new VariationDeleted(GetAggregateId(), value);
    Apply(@event);
  }
}