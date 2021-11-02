using DarkDispatcher.Core;
using DarkDispatcher.Domain.SeedWork;

namespace DarkDispatcher.Domain.Features
{
  public record RuleComparerGroup(int Id, string Name) : Enumeration(Id, Name)
  {
    public static readonly RuleComparerGroup List = new(1, "List");
    public static readonly RuleComparerGroup Text = new(2, "Text");
    public static readonly RuleComparerGroup Number = new(3, "Number");
  }
}