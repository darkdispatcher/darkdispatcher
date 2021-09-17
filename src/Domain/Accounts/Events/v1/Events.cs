using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Domain.Accounts.Events.v1
{
  public record OrganizationCreated(string Id, string Name) : IDomainEvent;

  public record OrganizationDeleted(string Id) : IDomainEvent;

  public record OrganizationUpdated(string Id, string Name) : IDomainEvent;
}