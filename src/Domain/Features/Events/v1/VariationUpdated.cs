using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Features.Ids;

namespace DarkDispatcher.Domain.Features.Events.v1;

public record VariationUpdated(FeatureId FeatureId, string Id, string Value, string? Name = null, string? Description = null) : DomainEvent;