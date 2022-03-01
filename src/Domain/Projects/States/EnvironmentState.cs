using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Projects.Entities;
using DarkDispatcher.Domain.Projects.Events.v1;
using DarkDispatcher.Domain.Projects.Ids;

namespace DarkDispatcher.Domain.Projects.States;

public record EnvironmentState : AggregateState<EnvironmentState, EnvironmentId>
{
  public EnvironmentState()
  {
    On<EnvironmentCreated>((state, added) => state with
    {
      Id = added.EnvironmentId,
      Name = added.Name,
      Color = EnvironmentColor.FindColorOrDefault(added.Color),
      Description = added.Description
    });

    On<EnvironmentUpdated>((state, updated) => state with
    {
      Name = updated.Name,
      Color = EnvironmentColor.FindColorOrDefault(updated.Color),
      Description = updated.Description
    });

    On<EnvironmentDeleted>((state, _) => state with
    {
      IsDeleted = true
    });
  }

  public string Name { get; init; } = null!;
  public string? Description { get; init; }
  public bool IsDeleted { get; init; }
  public EnvironmentColor Color { get; init; } = null!;
}
