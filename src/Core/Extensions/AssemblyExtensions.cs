using System.Reflection;

namespace DarkDispatcher.Core.Extensions;

public static class AssemblyExtensions
{
  /// <summary>
  /// Gets Informational Version of assembly
  /// </summary>
  /// <param name="assembly"></param>
  /// <returns></returns>
  public static string GetInformationalVersion(this Assembly? assembly)
  {
    return assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unknown";
  }

  /// <summary>
  /// Gets Product Name of assembly
  /// </summary>
  /// <param name="assembly"></param>
  /// <returns></returns>
  public static string GetProductName(this Assembly? assembly)
  {
    return assembly?.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? string.Empty;
  }
}
