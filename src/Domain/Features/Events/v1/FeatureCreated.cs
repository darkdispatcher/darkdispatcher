using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Features.Entities;
using DarkDispatcher.Domain.Features.Ids;
using DarkDispatcher.Domain.Projects.Entities;

namespace DarkDispatcher.Domain.Features.Events.v1;

public record FeatureCreated(
  FeatureId FeatureId, 
  string Key, 
  string Name, 
  VariationType Type,
  Variation[] Variations, 
  RuleVariationDefaults Defaults,
  Tag[] Tags = null,
  string? Description = null) : DomainEvent;