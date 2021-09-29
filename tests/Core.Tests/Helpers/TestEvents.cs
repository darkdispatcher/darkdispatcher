using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Core.Tests.Helpers
{
  internal static class TestEvents
  {
    public record TestAggregateCreated(string Id, string Name) : IDomainEvent;
    public record TestAggregateUpdated(string Id, string Name) : IDomainEvent;
    public record TestAggregateDeleted(string Id) : IDomainEvent;
    public record Invalid(string Id) : IDomainEvent;
  }
}