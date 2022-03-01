using MediatR;

namespace DarkDispatcher.Core.Events;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
  where TEvent : DomainEvent
{
}
