using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Core.Tests.Helpers
{
  internal record TestAggregateState : AggregateState<TestAggregateState, TestAggregateId>
  {
    public string Name { get; init; }
    public bool IsDeleted { get; init; }

    public override TestAggregateState When(IDomainEvent @event)
    {
      return @event switch
      {
        TestEvents.TestAggregateCreated created => this with
        {
          Id = new TestAggregateId(created.Id),
          Name = created.Name
        },
        TestEvents.TestAggregateUpdated updated => this with { Name = updated.Name }, 
        TestEvents.TestAggregateDeleted => this with { IsDeleted = true },
        _ => this
      };
    }
  }
}