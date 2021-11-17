using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Accounts;

namespace DarkDispatcher.Domain.Projects.Ids;

public record ProjectId(OrganizationId OrganizationId, string Value) : AggregateId(OrganizationId.Value, Value);