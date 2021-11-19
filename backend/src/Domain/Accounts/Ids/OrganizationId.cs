using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Domain.Accounts.Ids;

public record OrganizationId(string Value) : AggregateId(Value);