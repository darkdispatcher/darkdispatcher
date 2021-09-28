using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DarkDispatcher.Infrastructure.Logging
{
  public static class Extensions
  {
    public static ILoggingBuilder AddLogging(this ILoggingBuilder builder, IConfiguration configuration)
    {
      var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

      Log.Logger = logger;
      builder.AddSerilog(logger);

      return builder;
    }
  }
}