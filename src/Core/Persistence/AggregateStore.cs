using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Core.Persistence
{
  public class AggregateStore : IAggregateStore
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
      await _eventStore
        .AddEventsAsync<TAggregate>(aggregate.GetId(), aggregate.Version, events, cancellationToken)
        .ConfigureAwait(false);
      aggregate.ClearChanges();

      return aggregate;
    }

    public async ValueTask<TAggregate> LoadAsync<TAggregate>(
      string aggregateId,
      long? version = null,
      CancellationToken cancellationToken = default)
      where TAggregate : Aggregate, new()
    {
      var aggregate = new TAggregate();
      await _eventStore.ReadStreamAsync(aggregateId, version ?? 0L, e => aggregate.Fold(e), cancellationToken);
      
      return aggregate;
    }
  }
}