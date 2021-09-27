namespace DarkDispatcher.Core.Domain
{
  public abstract record AggregateState<T> 
    where T : AggregateState<T>
  {
    public abstract T When(IDomainEvent @event);
  }

  public abstract record AggregateState<T, TId> : AggregateState<T>
    where T : AggregateState<T, TId>
    where TId : AggregateId
  {
    public TId Id { get; protected init; } = null!;

    internal T SetId(TId id) => (T)this with { Id = id };
  }
}