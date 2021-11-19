using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Accounts;
using DarkDispatcher.Domain.Accounts.Ids;

namespace DarkDispatcher.Domain.Projects.Ids;

public record ProjectId(OrganizationId OrganizationId, string Value) : AggregateId(OrganizationId.Value, Value);