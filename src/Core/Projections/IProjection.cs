using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Core.Projections
{
  public interface IProjection
  {
    void When(IDomainEvent @event);
  }
}