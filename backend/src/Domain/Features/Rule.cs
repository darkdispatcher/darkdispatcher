using System.Collections.Generic;

namespace DarkDispatcher.Domain.Features;

public record Rule(string Name, RuleServeType ServeType, string? Variation = null)
{
  public List<RuleCondition> Conditions { get; init; } = new();
  public List<VariationRollout>? Rollouts { get; init; } = new();
}