using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json.Serialization;

namespace DarkDispatcher.Core.Domain
{
  public abstract class Aggregate
  {
    private readonly ConcurrentQueue<IDomainEvent> _changes = new();
    public IReadOnlyCollection<IDomainEvent> Changes => _changes.ToImmutableList();

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
    public abstract void Load(IEnumerable<IDomainEvent?> events);

    /// <summary>
    /// The fold operation for events loaded from the store, which restores the aggregate state.
    /// </summary>
    /// <param name="event">Domain @event to be applied to the state</param>
    public abstract void Fold(IDomainEvent @event);

    /// <summary>
    /// Adds an @event to the list of pending changes.
    /// </summary>
    /// <param name="event">New domain @event</param>
    protected void AddChange(IDomainEvent @event) => _changes.Enqueue(@event);

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
    protected virtual (T PreviousState, T CurrentState) Apply(IDomainEvent @event)
    {
      AddChange(@event);
      var previous = State;
      State = State.When(@event);
      Version++;
      return (previous, State);
    }

    /// <inheritdoc />
    public override void Load(IEnumerable<IDomainEvent?> events)
      => State = events.Where(x => x != null).Aggregate(new T(), Fold!);

    /// <inheritdoc />
    public override void Fold(IDomainEvent @event) => State = Fold(State, @event);

    /// <summary>
    /// Returns the current aggregate state. Cannot be mutated from the outside.
    /// </summary>
    public T State { get; internal set; }

    private T Fold(T state, IDomainEvent evt)
    {
      Version++;
      return state.When(evt);
    }
  }
  
  public abstract class Aggregate<T, TId> : Aggregate<T>
    where T : AggregateState<T, TId>, new()
    where TId : AggregateId {
    /// <inheritdoc />
    public override string GetId() => State.Id;
  }
}