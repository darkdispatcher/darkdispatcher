using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Core.Projections
{
  public interface IProjection<in TEvent>
    where TEvent : class, IDomainEvent
  {
    void Apply(TEvent @event);
  }
}