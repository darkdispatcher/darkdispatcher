using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Core.Tests.Helpers
{
  internal record TestAggregateId(string Value) : AggregateId(Value);
}