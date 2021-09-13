using System;
using System.Collections.Generic;

namespace DarkDispatcher.Core.Domain
{
  public abstract record AggregateState<T> 
    where T : AggregateState<T>
  {
    private readonly Dictionary<Type, Func<IDomainEvent, T>> _handlers = new();

    public virtual T When(IDomainEvent @event)
    {
      var eventType = @event.GetType();
      if (!_handlers.ContainsKey(eventType)) return (T)this;

      return _handlers[eventType](@event);
    }

    protected void On<TEvent>(Func<TEvent, T> handle)
    {
      if (!_handlers.TryAdd(typeof(TEvent), x => handle((TEvent)x)))
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
}