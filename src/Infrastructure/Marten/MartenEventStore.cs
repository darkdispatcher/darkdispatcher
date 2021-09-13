using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Domain;
using DarkDispatcher.Core.Persistence;
using Marten;
using MediatR;

namespace DarkDispatcher.Infrastructure.Marten
{
  public class MartenEventStore : IEventStore
  {
    private readonly IDocumentStore _store;
    private readonly IMediator _mediator;

    public MartenEventStore(IDocumentStore store, IMediator mediator)
    {
      _store = store;
      _mediator = mediator;
    }

    public async Task AddEventsAsync<TAggregate>(
      string streamId,
      long expectedVersion,
      IReadOnlyCollection<IDomainEvent> events,
      CancellationToken cancellationToken = default)
      where TAggregate : Aggregate
    {
      await using var session = _store.OpenSession();
      
      session.Events.Append(streamId, expectedVersion, events);
      await session.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask<IDomainEvent[]> GetEventsAsync(
      string aggregateId,
      long startVersion = 0L,
      CancellationToken cancellationToken = default)
    {
      await using var session = _store.OpenSession();
      var events = await session.Events.FetchStreamAsync(aggregateId, startVersion, token: cancellationToken);
      var savedEvents = events.Select(x => (x.Data as IDomainEvent)!).ToArray();

      return savedEvents;
    }

    public async Task ReadStreamAsync(
      string streamId, long startVersion, Action<IDomainEvent> callback,
      CancellationToken cancellationToken = default)
    {
      await using var session = _store.OpenSession();
      var events = await session.Events.FetchStreamAsync(streamId, startVersion, token: cancellationToken);
      var savedEvents = events.Select(x => (x.Data as IDomainEvent)!).ToArray();

      foreach (var @event in savedEvents)
      {
        callback(@event);
      }
    }
  }
}