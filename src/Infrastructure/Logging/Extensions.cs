using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace DarkDispatcher.Infrastructure.Logging;

public static class Extensions
{
  public static ILoggingBuilder AddDefaultLogging(this ILoggingBuilder builder, IConfiguration configuration)
  {
    builder.ClearProviders();

    var logger = new LoggerConfiguration()
      .MinimumLevel.Verbose()
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
