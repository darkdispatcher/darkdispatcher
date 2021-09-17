using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Domain.Projects.Events.v1
{
  public record ProjectCreated(string TenantId, string Id, string Name, string Description) : IDomainEvent;
  public record ProjectUpdated(string TenantId, string Id, string Name, string Description) : IDomainEvent;
  public record ProjectDeleted(string TenantId, string Id) : IDomainEvent;
  public record TagCreated(string TenantId, string Id, string Name, string Color) : IDomainEvent;
  public record TagDeleted(string TenantId, string Id, string Name) : IDomainEvent;
  public record TagUpdated(string TenantId, string Id, string Name, string Color) : IDomainEvent;
}