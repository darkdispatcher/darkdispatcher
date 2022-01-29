using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Features.Ids;
using DarkDispatcher.Domain.Projects.Entities;

namespace DarkDispatcher.Domain.Features.Events.v1;

public record FeatureUpdated(FeatureId FeatureId, string Name, Tag[]? Tags = null, string? Description = null) : DomainEvent;
