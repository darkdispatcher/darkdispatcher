using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DarkDispatcher.Domain.SeedWork
{
  public abstract record Enumeration : IComparable
  {
    protected Enumeration(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public int Id { get; }
    public string Name { get; }
    public override string ToString() => Name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
      typeof(T).GetFields(BindingFlags.Public |
                          BindingFlags.Static |
                          BindingFlags.DeclaredOnly)
        .Select(f => f.GetValue(null))
        .Cast<T>();


    public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

    public static IEnumerable<T> Where<T>(Func<T, bool> predicate) where T : Enumeration =>
      GetAll<T>().Where(predicate);

    public static T? SingleOrDefault<T>(Func<T, bool> predicate) where T : Enumeration =>
      GetAll<T>().SingleOrDefault(predicate);
  }
}