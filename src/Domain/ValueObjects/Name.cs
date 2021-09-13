namespace DarkDispatcher.Domain.ValueObjects
{
  public record Name(string FirstName, string LastName)
  {
    public override string ToString()
    {
      return $"{FirstName} {LastName}";
    }
  }
}