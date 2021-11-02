using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Domain.Projects
{
  public static class ProjectEvents
  {
    public static class V1
    {
      public record ProjectCreated(ProjectId ProjectId, string Name, string Description) : DomainEvent;

      public record ProjectUpdated(ProjectId ProjectId, string Name, string Description) : DomainEvent;

      public record ProjectDeleted(ProjectId ProjectId, string Name) : DomainEvent;

      public record EnvironmentCreated(ProjectId ProjectId, string Id, string Name, string Description, string Color) : DomainEvent;

      public record EnvironmentUpdated(ProjectId ProjectId, string Id, string Name, string Description, string Color) : DomainEvent;

      public record EnvironmentDeleted(ProjectId ProjectId, string Id, string Name) : DomainEvent;

      public record TagCreated(ProjectId ProjectId, string Id, string Name, string Color) : DomainEvent;

      public record TagUpdated(ProjectId ProjectId, string Id, string Name, string Color) : DomainEvent;

      public record TagDeleted(ProjectId ProjectId, string Id, string Name) : DomainEvent;

      public record ConfigurationCreated(ConfigurationId ConfigurationId, string Name, string? Description) : DomainEvent;

      public record ConfigurationUpdated(ConfigurationId ConfigurationId, string Name, string? Description) : DomainEvent;

      public record ConfigurationDeleted(ConfigurationId ConfigurationId, string Name) : DomainEvent;
    }
  }
}