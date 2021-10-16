using DarkDispatcher.Core;
using DarkDispatcher.Domain.SeedWork;

namespace DarkDispatcher.Domain.Projects
{
  public record EnvironmentColor(int Id, string Name, string Value) : Enumeration(Id, Name)
  {
    public static EnvironmentColor Default => RaisinBlack;

    public static readonly EnvironmentColor FireEngineRed = new(1, "Fire Engine Red", "#c71726ff");
    public static readonly EnvironmentColor WindsorTan = new(2, "Windsor Tan", "#b15b29ff");
    public static readonly EnvironmentColor GreenPigment = new(3, "Green Pigment", "#20983cff");
    public static readonly EnvironmentColor BrightNavyBlue = new(4, "Bright Navy Blue", "#2877c1ff");
    public static readonly EnvironmentColor PersianIndigo = new(5, "Persian Indigo", "#3d1078ff");
    public static readonly EnvironmentColor Byzantine = new(6, "Byzantine", "#b3279eff");
    public static readonly EnvironmentColor Bistre = new(7, "Bistre", "#452b1cff");
    public static readonly EnvironmentColor RaisinBlack = new(8, "Raisin Black", "#1c1d23ff");

    public static EnvironmentColor FindColorOrDefault(string name) => SingleOrDefault<EnvironmentColor>(x => x.Name == name) ?? Default;
  }
}