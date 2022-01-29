using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Core.Events;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using Marten;

namespace DarkDispatcher.Infrastructure.Marten;

public class MartenEventStore : IEventStore
{
  private readonly IDocumentStore _store;

  public MartenEventStore(IDocumentStore store)
  {
    _store = store;
  }

  public async Task AddEventsAsync<TAggregate>(
    StreamId streamId,
    long expectedVersion,
    IReadOnlyCollection<DomainEvent> events,
    CancellationToken cancellationToken = default)
    where TAggregate : Aggregate
  {
    await using var session = GetSession(streamId);

    session.Events.Append(streamId.AggregateId, expectedVersion, events);
    await session.SaveChangesAsync(cancellationToken);
  }

  public async ValueTask<DomainEvent[]> GetEventsAsync(
    StreamId streamId,
    long startVersion = 0L,
    CancellationToken cancellationToken = default)
  {
    await using var session = GetSession(streamId);
    var events = await session.Events.FetchStreamAsync(streamId.AggregateId, startVersion, token: cancellationToken);
    var savedEvents = events.Select(x => (x.Data as DomainEvent)!).ToArray();

    return savedEvents;
  }

  public async Task ReadStreamAsync(
    StreamId streamId,
    long startVersion,
    Action<DomainEvent> callback,
    CancellationToken cancellationToken = default)
  {
    await using var session = GetSession(streamId);
    var events = await session.Events.FetchStreamAsync(streamId.AggregateId, startVersion, token: cancellationToken);
    var savedEvents = events.Select(x => (x.Data as DomainEvent)!).ToArray();

    foreach (var @event in savedEvents)
    {
      callback(@event);
    }
  }

  private IDocumentSession GetSession(StreamId streamId)
  {
    return !string.IsNullOrWhiteSpace(streamId.TenantId) ? _store.OpenSession(streamId.TenantId) : _store.OpenSession();
  }
}
