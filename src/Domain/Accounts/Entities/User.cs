using DarkDispatcher.Domain.ValueObjects;

namespace DarkDispatcher.Domain.Accounts.Entities;

public class User
{
  public string Id { get; set; } = null!;

  public Name Name { get; set; } = null!;

  public string Email { get; set; } = null!;
}
