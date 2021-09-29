using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Core.Events;

namespace DarkDispatcher.Core.Persistence
{
  public class InMemoryEventStore : IEventStore
  {
    private readonly ConcurrentQueue<EventData> _eventStore = new();

    public Task AddEventsAsync<TAggregate>(string streamId, long expectedVersion,
      IReadOnlyCollection<IDomainEvent> events,
      CancellationToken cancellationToken = default)
      where TAggregate : Aggregate
    {
      var list = _eventStore.Where(x => x.StreamId == streamId).ToList();
      var version = list.Any() ? list.Last().Version + 1 : 1;

      foreach (var @event in events)
      {
        _eventStore.Enqueue(new EventData
        {
          StreamId = streamId,
          Type = @event.GetType().Name,
          Version = version,
          Data = @event
        });

        version++;
      }

      return Task.CompletedTask;
    }

    public ValueTask<IDomainEvent[]> GetEventsAsync(string streamId, long startVersion = 0,
      CancellationToken cancellationToken = default)
    {
      var events = _eventStore.Where(x => x.StreamId == streamId && x.Version >= startVersion);
      var domainEvents = events.Select(x => x.Data as IDomainEvent).ToArray();
      return ValueTask.FromResult(domainEvents)!;
    }

    public Task ReadStreamAsync(string streamId, long startVersion, Action<IDomainEvent> callback,
      CancellationToken cancellationToken = default)
    {
      var events = _eventStore.Where(x => x.StreamId == streamId).Select(x => x.Data as IDomainEvent);
      foreach (var @event in events)
      {
        callback(@event!);
      }

      return Task.CompletedTask;
    }

    private record EventData
    {
      public string StreamId { get; init; } = null!;
      public long Version { get; init; }
      public string Type { get; set; } = null!;
      public object? Data { get; init; }
    }
  }
}