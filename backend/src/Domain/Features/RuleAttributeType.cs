using DarkDispatcher.Domain.SeedWork;

namespace DarkDispatcher.Domain.Features;

public record RuleAttributeType(int Id, string Name, bool RequiresComparisonValue = true) : Enumeration(Id, Name)
{
  public static readonly RuleAttributeType Anonymous = new(20, "Anonymous", false);
  public static readonly RuleAttributeType Identifier = new(21, "Identifier");
  public static readonly RuleAttributeType Email = new(22, "Email");
  public static readonly RuleAttributeType Country = new(23, "Country");
    
  public static readonly RuleAttributeType Custom = new(99, "Custom");
}