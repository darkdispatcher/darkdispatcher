namespace DarkDispatcher.Domain.Features;

public record RuleCondition(RuleAttributeType Attribute, RuleComparer Comparer, string[] Values);