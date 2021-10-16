using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Core.Tests.Helpers
{
  internal static class TestEvents
  {
    public record TestAggregateCreated(string Id, string Name) : DomainEvent;
    public record TestAggregateUpdated(string Id, string Name) : DomainEvent;
    public record TestAggregateDeleted(string Id) : DomainEvent;
    public record Invalid(string Id) : DomainEvent;
  }
}