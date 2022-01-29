using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DarkDispatcher.Core.Events;
using DarkDispatcher.Core.Persistence;

namespace DarkDispatcher.Core.Aggregates;

public abstract class Aggregate
{
  private readonly ConcurrentQueue<DomainEvent> _changes = new();

  public IReadOnlyCollection<DomainEvent> Changes => _changes.ToImmutableList();

  /// <summary>
  /// Get the tenant id in a storage-friendly format. Allows using a value object as the tenant id
  /// inside the model, which then gets converted to a string for storage purposes.
  /// </summary>
  /// <returns></returns>
  public abstract string? GetTenantId();

  /// <summary>
  /// Get the aggregate id in a storage-friendly format. Allows using a value object as the aggregate id
  /// inside the model, which then gets converted to a string for storage purposes.
  /// </summary>
  /// <returns></returns>
  public abstract string GetId();

  public long Version { get; protected set; }

  /// <summary>
  /// Restores the aggregate state from a collection of events, previously stored in the <seealso cref="AggregateStore"/>
  /// </summary>
  /// <param name="events">Domain events from the aggregate stream</param>
  public abstract void Load(IEnumerable<DomainEvent?> events);

  /// <summary>
  /// The fold operation for events loaded from the store, which restores the aggregate state.
  /// </summary>
  /// <param name="event">Domain @event to be applied to the state</param>
  public abstract void Fold(DomainEvent @event);

  /// <summary>
  /// Adds an @event to the list of pending changes.
  /// </summary>
  /// <param name="event">New domain @event</param>
  protected void AddChange(DomainEvent @event) => _changes.Enqueue(@event);

  /// <summary>
  /// Clears all the pending changes.
  /// </summary>
  public void ClearChanges() => _changes.Clear();
}

public abstract class Aggregate<T> : Aggregate where T : AggregateState<T>, new()
{
  protected Aggregate() => State = new T();

  /// <summary>
  /// Applies a new @event to the state, adds the @event to the list of pending changes,
  /// and increases the current version.
  /// </summary>
  /// <param name="event">New domain @event to be applied</param>
  /// <returns>The previous and the new aggregate states</returns>
  protected virtual (T PreviousState, T CurrentState) Apply(DomainEvent @event)
  {
    AddChange(@event);
    var previous = State;
    State = State.When(@event);
    Version++;
    return (previous, State);
  }

  /// <inheritdoc />
  public override void Load(IEnumerable<DomainEvent?> events)
    => State = events.Where(x => x != null).Aggregate(new T(), Fold!);

  /// <inheritdoc />
  public override void Fold(DomainEvent @event) => State = Fold(State, @event);

  /// <summary>
  /// Returns the current aggregate state. Cannot be mutated from the outside.
  /// </summary>
  public T State { get; internal set; }

  private T Fold(T state, DomainEvent evt)
  {
    Version++;
    return state.When(evt);
  }
}

public abstract class Aggregate<T, TId> : Aggregate<T>
  where T : AggregateState<T, TId>, new()
  where TId : AggregateId
{
  public TId GetAggregateId() => State.Id;
  public override string? GetTenantId() => State.Id.TenantId;
  public override string GetId() => State.Id.Value;

}
