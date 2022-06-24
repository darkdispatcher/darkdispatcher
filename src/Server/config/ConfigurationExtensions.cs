using Microsoft.Extensions.Configuration;

namespace DarkDispatcher.Server.Config;

public static class ConfigurationExtensions
{
  public static void ConfigureForDarkDispatcher(this IConfigurationBuilder builder)
  {
    builder.AddJsonFile("appsettings.Custom.json", true);
  }
}
