using DarkDispatcher.Core.Projections;

namespace DarkDispatcher.Core.Tests.Helpers;

internal class TestProjection2
  : IProjection<TestEvents.Invalid>
{
  public void Apply(TestEvents.Invalid @event)
  {

  }
}
