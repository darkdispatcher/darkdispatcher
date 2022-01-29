using System;
using System.Collections.Generic;
using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Core.Aggregates;

public abstract record AggregateState<T>
  where T : AggregateState<T>
{
  private readonly Dictionary<Type, Func<T, DomainEvent, T>> _handlers = new();

  public virtual T When(DomainEvent @event)
  {
    var eventType = @event.GetType();

    if (!_handlers.TryGetValue(eventType, out var handler))
      return (T)this;

    return handler((T)this, @event);
  }

  protected void On<TEvent>(Func<T, TEvent, T> handle)
    where TEvent : DomainEvent
  {
    if (!_handlers.TryAdd(typeof(TEvent), (state, @event) => handle(state, (TEvent)@event)))
    {
      throw new InvalidOperationException($"Duplicate handler for {typeof(TEvent).Name}");
    }
  }
}

public abstract record AggregateState<T, TId> : AggregateState<T>
  where T : AggregateState<T, TId>
  where TId : AggregateId
{
  public TId Id { get; protected init; } = null!;

  internal T SetId(TId id) => (T)this with { Id = id };
}
