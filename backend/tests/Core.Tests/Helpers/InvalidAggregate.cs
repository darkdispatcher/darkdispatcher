using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Core.Tests.Helpers;

internal record InvalidAggregateId(string Value) : AggregateId(Value);
  
internal sealed class InvalidAggregate : Aggregate<InvalidAggregateState, InvalidAggregateId>
{
  public InvalidAggregate(InvalidAggregateId id)
  {
    Apply(new TestEvents.Invalid(id.Value));
  }
}

internal record InvalidAggregateState : AggregateState<InvalidAggregateState, InvalidAggregateId>
{
  public InvalidAggregateState()
  {
    // register event twice to throw exception
    On<TestEvents.Invalid>((state, invalid) => state with { });
    On<TestEvents.Invalid>((state, invalid) => state with { });
  }
    
  public string Name { get; init; } = null!;
}