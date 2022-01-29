using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Core.Ids;

namespace DarkDispatcher.Core.Persistence;

internal class AggregateStore : IAggregateStore
{
  private readonly IEventStore _eventStore;

  public AggregateStore(IEventStore eventStore)
  {
    _eventStore = eventStore;
  }

  public async Task<TAggregate> StoreAsync<TAggregate>(TAggregate aggregate, CancellationToken cancellationToken = default)
    where TAggregate : Aggregate
  {
    var events = aggregate.Changes;
    var streamId = new StreamId(aggregate.GetTenantId(), aggregate.GetId());
    await _eventStore
      .AddEventsAsync<TAggregate>(streamId, aggregate.Version, events, cancellationToken)
      .ConfigureAwait(false);
    aggregate.ClearChanges();

    return aggregate;
  }

  public async ValueTask<TAggregate> LoadAsync<TAggregate>(
    AggregateId aggregateId,
    long? version = null,
    CancellationToken cancellationToken = default)
    where TAggregate : Aggregate, new()
  {
    var aggregate = new TAggregate();
    var streamId = new StreamId(aggregateId.TenantId, aggregateId.Value);
    await _eventStore.ReadStreamAsync(streamId, version ?? 0L, e => aggregate.Fold(e), cancellationToken);
      
    return aggregate;
  }
}