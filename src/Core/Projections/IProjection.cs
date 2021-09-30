using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Core.Projections
{
  public interface IView
  {
  }

  public interface IProjection<in TEvent> : IView
    where TEvent : class, IDomainEvent
  {
    void Apply(TEvent @event);
  }
}