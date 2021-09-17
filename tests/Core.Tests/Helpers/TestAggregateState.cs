using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Core.Tests.Helpers
{
  internal record TestAggregateState : AggregateState<TestAggregateState, TestAggregateId>
  {
    public string Name { get; init; }
    public bool IsDeleted { get; init; }

    public override TestAggregateState When(IDomainEvent @event)
    {
      base.When(@event);
      return @event switch
      {
        TestEvents.TestAggregateCreated created => this with
        {
          Id = new TestAggregateId(created.Id),
          Name = created.Name
        },
        TestEvents.TestAggregateDeleted => this with { IsDeleted = true },
        _ => this
      };
    }
  }
}