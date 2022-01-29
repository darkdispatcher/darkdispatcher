using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Projects.Ids;

namespace DarkDispatcher.Domain.Projects.Events.v1;

public record ConfigurationUpdated(ConfigurationId ConfigurationId, string Name, string? Description) : DomainEvent;
