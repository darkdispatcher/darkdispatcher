using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Core.Persistence;

public interface IAggregateStore
{
  /// <summary>
  /// Store the new or updated aggregate state
  /// </summary>
  /// <param name="aggregate">Aggregate instance, which needs to be persisted</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <typeparam name="TAggregate">Aggregate type</typeparam>
  /// <returns></returns>
  Task<TAggregate> StoreAsync<TAggregate>(TAggregate aggregate, CancellationToken cancellationToken = default)
    where TAggregate : Aggregate;

  /// <summary>
  /// Load the aggregate from the store for a given id
  /// </summary>
  /// <param name="aggregateId">Aggregate id</param>
  /// <param name="version">What version to load</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <typeparam name="TAggregate">Aggregate type</typeparam>
  /// <returns></returns>
  ValueTask<TAggregate> LoadAsync<TAggregate>(
    AggregateId aggregateId,
    long? version = null,
    CancellationToken cancellationToken = default)
    where TAggregate : Aggregate, new();
}
