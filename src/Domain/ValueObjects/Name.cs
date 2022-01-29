namespace DarkDispatcher.Domain.ValueObjects;

public record Name(string FirstName, string LastName)
{
  public override string ToString() => $"{FirstName} {LastName}";
}
