namespace DarkDispatcher.Infrastructure.GraphQL.Organizations
{
  /// <summary>
  /// An account on Dark Dispatcher, with one or more owners, that has projects, members and teams.
  /// </summary>
  public class Organization
  {
    /// <summary>
    /// Name of the Organization
    /// </summary>
    public string Name { get; init; } = null!;
  }
}