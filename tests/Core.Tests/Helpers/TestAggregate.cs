using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Core.Tests.Helpers;

internal record TestAggregateId(string Value) : AggregateId(Value);

internal sealed class TestAggregate : Aggregate<TestAggregateState, TestAggregateId>
{
  public TestAggregate(TestAggregateId id, string name)
  {
    var @event = new TestEvents.TestAggregateCreated(id.Value, name);
    Apply(@event);
  }

  public void Update(string name)
  {
    var @event = new TestEvents.TestAggregateUpdated(GetId(), name);
    Apply(@event);
  }

  public void Delete()
  {
    var @event = new TestEvents.TestAggregateDeleted(GetId());
    Apply(@event);
  }
}

internal record TestAggregateState : AggregateState<TestAggregateState, TestAggregateId>
{
  public TestAggregateState()
  {
    On<TestEvents.TestAggregateCreated>((state, created) => state with
    {
      Id = new TestAggregateId(created.Id),
      Name = created.Name
    });

    On<TestEvents.TestAggregateUpdated>((state, updated) => state with
    {
      Name = updated.Name
    });

    On<TestEvents.TestAggregateDeleted>((state, deleted) => state with
    {
      IsDeleted = true
    });
  }

  public string Name { get; init; } = null!;
  public bool IsDeleted { get; init; }
}
