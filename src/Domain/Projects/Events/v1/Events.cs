using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Domain.Projects.Events.v1
{
  public record ProjectCreated(ProjectId ProjectId, string Name, string Description) : IDomainEvent;
  public record ProjectUpdated(ProjectId ProjectId, string Name, string Description) : IDomainEvent;
  public record ProjectDeleted(ProjectId ProjectId, string name) : IDomainEvent;
  public record EnvironmentCreated(ProjectId ProjectId, string Id, string Name, string Description, string Color) : IDomainEvent;
  public record EnvironmentUpdated(ProjectId ProjectId, string Id, string Name, string Description, string Color) : IDomainEvent;
  public record EnvironmentDeleted(ProjectId ProjectId, string Id, string Name) : IDomainEvent;
  public record TagCreated(ProjectId ProjectId, string Id, string Name, string Color) : IDomainEvent;
  public record TagUpdated(ProjectId ProjectId, string Id, string Name, string Color) : IDomainEvent;
  public record TagDeleted(ProjectId ProjectId, string Id, string Name) : IDomainEvent;
  public record ConfigurationCreated(ConfigurationId ConfigurationId, string Name, string Description) : IDomainEvent;
  public record ConfigurationUpdated(ConfigurationId ConfigurationId, string Name, string Description) : IDomainEvent;
  public record ConfigurationDeleted(ConfigurationId ConfigurationId, string Name) : IDomainEvent;
  public record FeatureCreated(FeatureId FeatureId, string Key, string Name, string Description) : IDomainEvent;
  public record FeatureUpdated(FeatureId FeatureId, string Key, string Name, string? Description) : IDomainEvent;
}