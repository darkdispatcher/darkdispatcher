using DarkDispatcher.Core.Projections;

namespace DarkDispatcher.Core.Tests.Helpers;

internal class TestProjection1
  : IProjection<TestEvents.TestAggregateCreated>
{
  public void Apply(TestEvents.TestAggregateCreated @event)
  {

  }
}
