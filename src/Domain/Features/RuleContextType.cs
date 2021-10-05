using DarkDispatcher.Core;

namespace DarkDispatcher.Domain.Features
{
  public record RuleContextType(int Id, string Name) : Enumeration(Id, Name)
  {
    public static readonly RuleContextType Identifier = new(1, "Identifier");
    public static readonly RuleContextType Email = new(2, "Email");
    
    public static readonly RuleContextType Custom = new(99, "Custom");
  }
}