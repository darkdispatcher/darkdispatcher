using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Projects.Ids;

namespace DarkDispatcher.Domain.Features.Ids;

public record FeatureId(ConfigurationId ConfigurationId, string Value) : AggregateId(ConfigurationId.TenantId, Value);