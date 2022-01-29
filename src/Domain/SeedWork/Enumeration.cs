using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DarkDispatcher.Domain.SeedWork;

public abstract record Enumeration(int Id, string Name) : IComparable
{
  public sealed override string ToString() => Name;

  public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
    typeof(T).GetFields(BindingFlags.Public |
                        BindingFlags.Static |
                        BindingFlags.DeclaredOnly)
      .Select(f => f.GetValue(null))
      .Cast<T>();

  public int CompareTo(object obj) => Id.CompareTo(((Enumeration)obj).Id);

  public static IEnumerable<T> Where<T>(Func<T, bool> predicate) where T : Enumeration =>
    GetAll<T>().Where(predicate);

  public static T? SingleOrDefault<T>(Func<T, bool> predicate) where T : Enumeration =>
    GetAll<T>().SingleOrDefault(predicate);
}