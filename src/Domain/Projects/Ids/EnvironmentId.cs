using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Domain.Projects.Ids;

public record EnvironmentId(ProjectId ProjectId, string Value) : AggregateId(ProjectId.TenantId, Value);
