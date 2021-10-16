namespace DarkDispatcher.Domain.ValueObjects
{
  public record Address(
    string Address1,
    string City, 
    string State, 
    string PostalCode, 
    string Country,
    string? Address2 = null)
  {
    public override string ToString() => $"{Address1}\n{Address2}\n{City}, {State} {PostalCode}\n{Country}";
  }
}