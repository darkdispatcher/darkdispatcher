namespace DarkDispatcher.Domain.Features.Entities;

public record RuleCondition(RuleAttributeType Attribute, RuleComparer Comparer, string[] Values);