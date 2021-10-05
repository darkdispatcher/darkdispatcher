namespace DarkDispatcher.Domain.Features
{
  public record Rule(RuleContextType ContextType, RuleComparer Comparer, string ComparisonValue, bool Value, string? CustomValue = null);
}