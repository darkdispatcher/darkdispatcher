using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Core.Tests.Helpers
{
  internal record TestAggregateId(string Value) : AggregateId(Value);
}