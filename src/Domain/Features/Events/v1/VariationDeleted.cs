using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Features.Ids;

namespace DarkDispatcher.Domain.Features.Events.v1;

public record VariationDeleted(FeatureId FeatureId, string Value) : DomainEvent;