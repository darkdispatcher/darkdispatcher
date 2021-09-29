namespace DarkDispatcher.Core.Aggregates
{
  public abstract record AggregateId(string Value)
  {
    public override string ToString() => Value;

    public static implicit operator string(AggregateId id) => id.Value;
    
    public void Deconstruct(out string value) => value = Value;
  }
}