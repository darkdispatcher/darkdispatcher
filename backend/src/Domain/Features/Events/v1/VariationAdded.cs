using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Features.Ids;

namespace DarkDispatcher.Domain.Features.Events.v1;

public record VariationAdded(FeatureId FeatureId, string Id, string Value, string? Name = null, string? Description = null) : DomainEvent;