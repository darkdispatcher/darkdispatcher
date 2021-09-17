using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Core.Tests.Helpers
{
  internal class TestAggregate : Aggregate<TestAggregateState, TestAggregateId>
  {
    public TestAggregate()
    {}
    
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