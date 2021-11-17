using DarkDispatcher.Domain.SeedWork;

namespace DarkDispatcher.Domain.Projects.Models;

public record TagColor(int Id, string Name, string Value) : Enumeration(Id, Name)
{
  public static TagColor Default => Concrete;
    
  public static TagColor Salmon = new(1, nameof(Salmon), "#fadbd8");
  public static TagColor Plum = new(2, nameof(Plum), "#e8daef");
  public static TagColor River = new(3, nameof(River), "#d6eaf8");
  public static TagColor Turquoise = new(4, nameof(Turquoise), "#d1f2eb");
  public static TagColor Sunflower = new(5, nameof(Sunflower), "#f9e79f");
  public static TagColor Carrot = new(6, nameof(Carrot), "#f5cba7");
  public static TagColor Concrete = new(7, nameof(Concrete), "#d5dbdb");
    
  public static TagColor FindColorOrDefault(string name) => SingleOrDefault<TagColor>(x => x.Name == name) ?? Default;
}