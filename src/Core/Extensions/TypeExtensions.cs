using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DarkDispatcher.Core.Extensions;

public static class TypeExtensions
{
  /// <summary>
  /// Get all types of generic type in Dark Dispatcher assemblies
  /// </summary>
  /// <returns>Types that inherit from generic type</returns>
  public static IReadOnlyCollection<Type> GetAllTypesImplementingOpenGenericType(this Type type)
  {
    var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName!.StartsWith("DarkDispatcher"));

    var allTypes = assemblies.SelectMany(x => x.GetTypes());
    var types = allTypes
      .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type) && !x.IsInterface && !x.IsAbstract)
      .ToImmutableList();

    return types;
  }
}