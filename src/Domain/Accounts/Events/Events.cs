using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Domain.Accounts.Events
{
  public record OrganizationCreated(string OrganizationId, string Name) : IDomainEvent;
  public record OrganizationDeleted(string OrganizationId) : IDomainEvent;
  public record OrganizationUpdated(string OrganizationId, string Name) : IDomainEvent;
}