namespace DarkDispatcher.Core.Ids
{
  public record StreamId(string Value)
  {
    public static StreamId For<T>(string aggregateId) => new($"{typeof(T).Name}-{aggregateId}");
    
    public static implicit operator string(StreamId streamId) => streamId.Value;

    public override string ToString() => Value;
  }
}