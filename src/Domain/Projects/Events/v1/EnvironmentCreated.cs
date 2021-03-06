using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Projects.Ids;

namespace DarkDispatcher.Domain.Projects.Events.v1;

public record EnvironmentCreated(EnvironmentId EnvironmentId, string Name, string Description, string Color) : DomainEvent;
