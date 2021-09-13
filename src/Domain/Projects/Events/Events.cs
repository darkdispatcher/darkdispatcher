using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Domain.Projects.Events
{
  public record ProjectCreated(string TenantId, string AggregateId, string Name, string Description) : IDomainEvent
  {
    public long Version => 1;
  }
  public record ProjectUpdated(string TenantId, string AggregateId, long Version, string Name, string Description) : IDomainEvent;
  public record TagCreated(string TenantId, string AggregateId, long Version, string Name, string Color) : IDomainEvent;
  public record TagDeleted(string TenantId, string AggregateId, long Version, string Name) : IDomainEvent;
  public record TagUpdated(string TenantId, string AggregateId, long Version, string Name, string Color) : IDomainEvent;
}