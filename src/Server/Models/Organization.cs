namespace DarkDispatcher.Server.Models;

/// <summary>
/// An account on Dark Dispatcher, with one or more owners, that has projects, members and teams.
/// </summary>
public class Organization
{
  public string Id { get; set; } = null!;

  /// <summary>
  /// Name of the Organization
  /// </summary>
  public string Name { get; set; } = null!;
}
