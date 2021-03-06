using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DarkDispatcher.Infrastructure.Logging;

public static class Extensions
{
  public static ILoggingBuilder ConfigureForDarkDispatcher(this ILoggingBuilder builder, IConfiguration configuration)
  {
    builder.ClearProviders();

    var logger = new LoggerConfiguration()
      .ReadFrom.Configuration(configuration)
      .Enrich.FromLogContext()
      .Enrich.WithEnvironmentName()
      .Enrich.WithMachineName()
      .Enrich.WithProcessId()
      .Enrich.WithThreadId()
      .WriteTo.Console()
      .CreateLogger();

    Log.Logger = logger;
    builder.AddSerilog(logger);

    return builder;
  }
}
