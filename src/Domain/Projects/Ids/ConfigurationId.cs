using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Domain.Projects.Ids;

public record ConfigurationId(string OrganizationId, string ProjectId, string Value) : AggregateId(OrganizationId, Value);
