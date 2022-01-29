using System.Collections.Generic;
using System.Linq;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Features.Entities;
using DarkDispatcher.Domain.Features.Events.v1;
using DarkDispatcher.Domain.Features.Ids;
using DarkDispatcher.Domain.Projects.Entities;

namespace DarkDispatcher.Domain.Features.States;

public record FeatureState : AggregateState<FeatureState, FeatureId>
{
  public FeatureState()
  {
    On<FeatureCreated>((state, created) => state with
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

    On<FeatureUpdated>((state, updated) => state with
    {
      Name = updated.Name,
      Description = updated.Description,
      Tags = updated.Tags?.ToList() ?? new List<Tag>()
    });

    On<VariationAdded>((state, added) =>
    {
      var variation = new Variation(added.Id, added.Value, added.Name, added.Description);
      state.Variations.Add(variation);

      return state;
    });

    On<VariationUpdated>((state, updated) =>
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

    On<VariationDeleted>((state, deleted) =>
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