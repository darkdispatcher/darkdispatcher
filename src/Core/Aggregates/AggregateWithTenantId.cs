namespace DarkDispatcher.Core.Aggregates
{
  public abstract record AggregateWithTenantId(string TenantId, string Value) : AggregateId(Value)
  {
    public override string ToString() => $"{TenantId}:{Value}";

    public static implicit operator string(AggregateWithTenantId id) => $"{id.TenantId}:{id.Value}";
    
    public void Deconstruct(out string tenantId, out string value)
    {
      tenantId = TenantId;
      value = Value;
    }
  }
}