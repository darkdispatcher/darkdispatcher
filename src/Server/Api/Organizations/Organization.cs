namespace DarkDispatcher.Server.Api.Organizations;

/// <summary>
/// An account on Dark Dispatcher, with one or more owners, that has projects, members and teams.
/// </summary>
public class Organization
{
  /// <summary>
  /// Id of the Organization
  /// </summary>
  public string Id { get; set; } = null!;

  /// <summary>
  /// Name of the Organization
  /// </summary>
  public string Name { get; set; } = null!;
}
