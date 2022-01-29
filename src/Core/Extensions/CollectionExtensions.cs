using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkDispatcher.Core.Extensions;

public static class CollectionExtensions
{
  public static bool HasDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
  {
    var duplicates = source
      .GroupBy(keySelector)
      .Where(g => g.Count() > 1)
      .Select(g => g.Key);

    return duplicates.Any();
  }
}
