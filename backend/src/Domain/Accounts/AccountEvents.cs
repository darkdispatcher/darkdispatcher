using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Domain.Accounts;

public static class AccountEvents
{
  public static class V1
  {
    public record OrganizationCreated(string Id, string Name) : DomainEvent;

    public record OrganizationDeleted(string Id) : DomainEvent;

    public record OrganizationUpdated(string Id, string Name) : DomainEvent;
  }
}