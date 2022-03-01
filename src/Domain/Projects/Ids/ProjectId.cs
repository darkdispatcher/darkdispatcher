using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Domain.Projects.Ids;

public record ProjectId(string TenantId, string Value) : AggregateId(TenantId, Value);
