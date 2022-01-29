using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Projects.Events.v1;
using DarkDispatcher.Domain.Projects.Ids;

namespace DarkDispatcher.Domain.Projects.States;

public record ConfigurationState : AggregateState<ConfigurationState, ConfigurationId>
{
  public ConfigurationState()
  {
    On<ConfigurationCreated>((state, created) => state with
    {
      Id = created.ConfigurationId,
      Name = created.Name,
      Description = created.Description
    });

    On<ConfigurationUpdated>((state, updated) => state with
    {
      Name = updated.Name,
      Description = updated.Description
    });

    On<ConfigurationDeleted>((state, deleted) => state with
    {
      IsDeleted = true
    });
  }

  public string Name { get; init; } = null!;

  public string? Description { get; init; }

  public bool IsDeleted { get; init; }
}
