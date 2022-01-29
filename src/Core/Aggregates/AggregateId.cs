namespace DarkDispatcher.Core.Aggregates;

public abstract record AggregateId(string Value)
{
  protected AggregateId(string tenantId, string value) : this(value)
  {
    TenantId = tenantId;
  }

  public string? TenantId { get; }

  public override string ToString() => $"{TenantId}:{Value}";
}