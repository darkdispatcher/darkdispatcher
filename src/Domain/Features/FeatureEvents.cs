using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Domain.Features
{
  public static class FeatureEvents
  {
    public static class V1
    {
      public record FeatureCreated(FeatureId FeatureId, string Key, string Name, string? Description) : IDomainEvent;
      public record FeatureUpdated(FeatureId FeatureId, string Key, string Name, string? Description) : IDomainEvent;
    }
  }
}