namespace DarkDispatcher.Core.Aggregates;

public abstract record AggregateId
{
  protected AggregateId(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      throw new Exceptions.InvalidIdException(this);

    Value = value;
  }

  protected AggregateId(string tenantId, string value) : this(value)
  {
    TenantId = tenantId;
  }

  public string Value { get; }

  public string? TenantId { get; }

  public override string ToString() => $"{TenantId}:{Value}";
}
