using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Core.Events;
using DarkDispatcher.Core.Ids;

namespace DarkDispatcher.Core.Persistence
{
  public interface IEventStore
  {
    /// <summary>
    /// Append one or more events to a stream
    /// </summary>
    /// <param name="streamId">Stream name</param>
    /// <param name="expectedVersion">Expected stream version (can be Any)</param>
    /// <param name="events">Collection of events to append</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Append result, which contains the global position of the last written event,
    /// as well as the next stream version</returns>
    Task AddEventsAsync<TAggregate>(
      StreamId streamId,
      long expectedVersion,
      IReadOnlyCollection<IDomainEvent> events,
      CancellationToken cancellationToken = default)
      where TAggregate : Aggregate;

    /// <summary>
    /// Read a fixed number of events from an existing stream to an array
    /// </summary>
    /// <param name="streamId">Stream name</param>
    /// <param name="startVersion">Where to start reading events</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An array with events retrieved from the stream</returns>
    ValueTask<IDomainEvent[]> GetEventsAsync(
      StreamId streamId,
      long startVersion = 0L,
      CancellationToken cancellationToken = default);

    /// <summary>
    /// Read events from a stream asynchronously, calling a given function for each retrieved event
    /// </summary>
    /// <param name="streamId">Stream Id</param>
    /// <param name="startVersion">Where to start reading events</param>
    /// <param name="callback">A function to be called for retrieved each event</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task ReadStreamAsync(
      StreamId streamId,
      long startVersion,
      Action<IDomainEvent> callback,
      CancellationToken cancellationToken = default
    );
  }
}