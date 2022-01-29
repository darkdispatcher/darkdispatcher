using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Domain.Projects.Ids;

public record ConfigurationId(ProjectId ProjectId, string Value) : AggregateId(ProjectId.TenantId, Value);