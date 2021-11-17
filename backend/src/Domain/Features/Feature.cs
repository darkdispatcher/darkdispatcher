using System;
using System.Collections.Generic;
using System.Linq;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Core.Extensions;
using DarkDispatcher.Domain.Projects;
using DarkDispatcher.Domain.Projects.Ids;
using DarkDispatcher.Domain.Projects.Models;

namespace DarkDispatcher.Domain.Features;

public record FeatureId(ConfigurationId ConfigurationId, string Value) : AggregateId(ConfigurationId.TenantId, Value);

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

    var @event = new FeatureEvents.V1.FeatureCreated(id, key, name, type, variations, defaults, tags, description);
    Apply(@event);
  }

  public void Update(string name, Tag[]? tags = null, string? description = null)
  {
    var @event = new FeatureEvents.V1.FeatureUpdated(GetAggregateId(), name, tags, description);
    Apply(@event);
  }

  public void AddVariation(string id, string value, string? name = null, string? description = null)
  {
    if (State.Type == VariationType.Boolean)
      throw new InvalidOperationException($"Adding a variation for type '{nameof(VariationType.Boolean)}' is not allowed.");

    if (State.Variations.Any(x => x.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
      throw new InvalidOperationException("All variation values must be unique.");

    var @event = new FeatureEvents.V1.VariationAdded(GetAggregateId(), id, value, name, description);
    Apply(@event);
  }

  public void UpdateVariation(string id, string value, string? name = null, string? description = null)
  {
    var values = new[] { "true", "false" };
    if (State.Type == VariationType.Boolean && !values.Any(x => x.Equals(value, StringComparison.InvariantCulture)))
      throw new InvalidOperationException($"Only '{string.Join(',', values)}' are accepted for '{nameof(VariationType.Boolean)}' variation values.");

    if (State.Variations.Any(x => x.Id != id && x.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
      throw new InvalidOperationException("All variation values must be unique.");

    var @event = new FeatureEvents.V1.VariationUpdated(GetAggregateId(), id, value, name, description);
    Apply(@event);
  }

  public void DeleteVariation(string value)
  {
    if (State.Type == VariationType.Boolean)
      throw new InvalidOperationException(
        $"Deleting variations for type '{nameof(VariationType.Boolean)}' is not allowed.");

    var @event = new FeatureEvents.V1.VariationDeleted(GetAggregateId(), value);
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
      Description = created.Description,
      Type = created.Type,
      Variations = created.Variations.ToList(),
      OnVariation = created.Defaults.On,
      OffVariation = created.Defaults.Off
    });

    On<FeatureEvents.V1.FeatureUpdated>((state, updated) => state with
    {
      Name = updated.Name,
      Description = updated.Description,
      Tags = updated.Tags?.ToList() ?? new List<Tag>()
    });

    On<FeatureEvents.V1.VariationAdded>((state, added) =>
    {
      var variation = new Variation(added.Id, added.Value, added.Name, added.Description);
      state.Variations.Add(variation);

      return state;
    });

    On<FeatureEvents.V1.VariationUpdated>((state, updated) =>
    {
      var index = state.Variations.FindIndex(x => x.Id == updated.Id);
      if (index >= 0)
      {
        state.Variations[index] = Variations[index] with
        {
          Value = updated.Value,
          Name = updated.Name,
          Description = updated.Description
        };
      }

      return state;
    });

    On<FeatureEvents.V1.VariationDeleted>((state, deleted) =>
    {
      var variation = state.Variations.SingleOrDefault(x => x.Value == deleted.Value);
      if (variation != null)
        state.Variations.Remove(variation);

      return state;
    });
  }

  public string Key { get; init; } = null!;
  public string Name { get; init; } = null!;
  public string? Description { get; init; }
  public VariationType Type { get; init; }
  public string OnVariation { get; init; }
  public string OffVariation { get; init; }
  public List<Variation> Variations { get; init; } = new();
  public List<Tag> Tags { get; init; } = new();
}