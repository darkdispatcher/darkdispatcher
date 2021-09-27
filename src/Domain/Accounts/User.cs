using DarkDispatcher.Core.Domain;
using DarkDispatcher.Domain.ValueObjects;

namespace DarkDispatcher.Domain.Accounts
{
  public class User : IEntity<string>
  {
    public string Id { get; }

    public Name Name { get; set; }

    public string Email { get; set; }
  }
}