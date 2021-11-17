using DarkDispatcher.Core;
using DarkDispatcher.Domain.SeedWork;

namespace DarkDispatcher.Domain.Features;

public record RuleComparer(int Id, string Name, RuleComparerGroup Group) : Enumeration(Id, Name)
{
  public static readonly RuleComparer IsOneOf = new(1, "Is One Of", RuleComparerGroup.List);
  public static readonly RuleComparer IsNotOneOf = new(2, "Is Not One Of", RuleComparerGroup.List);
    
  public static readonly RuleComparer Contains = new(3, "Contains", RuleComparerGroup.Text);
  public static readonly RuleComparer DoesNotContain = new(4, "Does Not Contain", RuleComparerGroup.Text);
    
  public static readonly RuleComparer Equal = new(5, "=", RuleComparerGroup.Number);
  public static readonly RuleComparer DoesNotEqual = new(6, "!=", RuleComparerGroup.Number);
  public static readonly RuleComparer LessThan = new(7, "<", RuleComparerGroup.Number);
  public static readonly RuleComparer LessThanOrEqualTo = new(8, "<=", RuleComparerGroup.Number);
  public static readonly RuleComparer GreaterThan = new(9, ">", RuleComparerGroup.Number);
  public static readonly RuleComparer GreaterThanOrEqualTo = new(10, ">=", RuleComparerGroup.Number);
}