using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Projects.Ids;

namespace DarkDispatcher.Domain.Projects.Events.v1;

public record TagCreated(ProjectId ProjectId, string Id, string Name, string Color) : DomainEvent;