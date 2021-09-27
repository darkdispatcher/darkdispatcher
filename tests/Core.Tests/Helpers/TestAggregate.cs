using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Core.Tests.Helpers
{
  internal sealed class TestAggregate : Aggregate<TestAggregateState, TestAggregateId>
  {
    public TestAggregate(TestAggregateId id, string name)
    {
      var @event = new TestEvents.TestAggregateCreated(id, name);
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
}