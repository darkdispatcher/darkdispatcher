using System;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Core.Exceptions
{
  internal class NoEventHandlerRegisteredException<TState> : Exception
    where TState : AggregateState<TState>
  {
    public NoEventHandlerRegisteredException(IDomainEvent @event) 
      : base ($"No Event Applier Registered For {@event.GetType().Name} on {typeof(TState).Name}")
    {
    }
  }
}