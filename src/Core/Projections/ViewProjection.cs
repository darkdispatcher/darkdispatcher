using System;
using System.Collections.Generic;
using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Core.Projections
{
  [Obsolete("TODO: Not Ready for Primetime.", true)]
  public abstract class ViewProjection
  {
    public Dictionary<Type, Action<IDomainEvent>> Handlers { get; } = new();

    protected void On<TEvent>(Action<TEvent> handle)
      where TEvent : class, IDomainEvent
    {
      if (!Handlers.TryAdd(typeof(TEvent), x => handle((TEvent) x))) {
        throw new InvalidOperationException($"Duplicate handler for {typeof(TEvent).Name}");
      }
    }
  }
}