using System;
using MediatR;

namespace DarkDispatcher.Core.Events;

public abstract record DomainEvent : INotification
{
  protected DomainEvent()
  {
    Timestamp = DateTimeOffset.Now;
  }

  public DateTimeOffset Timestamp { get; }
}