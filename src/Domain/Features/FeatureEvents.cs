using System.Collections.Generic;
using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Projects;

namespace DarkDispatcher.Domain.Features
{
  public static class FeatureEvents
  {
    public static class V1
    {
      public record FeatureCreated(
        FeatureId FeatureId, 
        string Key, 
        string Name, 
        VariationType Type,
        Variation[] Variations, 
        RuleVariationDefaults Defaults,
        Tag[] Tags = null,
        string? Description = null) : DomainEvent;

      public record FeatureUpdated(FeatureId FeatureId, string Name, Tag[]? Tags = null, string? Description = null) : DomainEvent;

      public record VariationAdded(FeatureId FeatureId, string Id, string Value, string? Name = null, string? Description = null) : DomainEvent;
      public record VariationUpdated(FeatureId FeatureId, string Id, string Value, string? Name = null, string? Description = null) : DomainEvent;
      public record VariationDeleted(FeatureId FeatureId, string Value) : DomainEvent;
    }
  }
}