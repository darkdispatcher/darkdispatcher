using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Projects.Ids;

namespace DarkDispatcher.Domain.Projects.Events.v1;

public record TagDeleted(ProjectId ProjectId, string Id, string Name) : DomainEvent;