using DarkDispatcher.Domain.SeedWork;

namespace DarkDispatcher.Domain.Features.Entities;

public record RuleServeType(int Id, string Name) : Enumeration(Id, Name)
{
  public static readonly RuleServeType Variant = new(1, "variant");
  public static readonly RuleServeType Percentage = new(2, "percentage rollout");
}
