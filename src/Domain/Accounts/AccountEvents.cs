using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Domain.Accounts
{
  public static class AccountEvents
  {
    public static class V1
    {
      public record OrganizationCreated(string Id, string Name) : IDomainEvent;

      public record OrganizationDeleted(string Id) : IDomainEvent;

      public record OrganizationUpdated(string Id, string Name) : IDomainEvent;
    }
  }
}