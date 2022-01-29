using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Projects.Ids;

namespace DarkDispatcher.Domain.Projects.Events.v1;

public record ProjectUpdated(ProjectId ProjectId, string Name, string Description) : DomainEvent;
