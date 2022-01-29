using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DarkDispatcher.Core;
using DarkDispatcher.Infrastructure;
using DarkDispatcher.Infrastructure.Logging;
using DarkDispatcher.Infrastructure.Marten.Identity;
using DarkDispatcher.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Server;

public static class StartupExtensions
{
  public static WebApplicationBuilder AddDarkDispatcher(this WebApplicationBuilder builder)
  {
    builder.Logging.AddDefaultLogging(builder.Configuration);

    // Services
    builder.Services
      .AddDarkDispatcherCore(builder.Configuration)
      .AddInfrastructure(builder.Environment)
      .AddMartenIdentity();

    builder.Services.AddGrpc(options =>
    {
      options.EnableDetailedErrors = true;
    });

    builder.StartupBanner();

    return builder;
  }

  public static Task RunDarkDispatcherAsync(this WebApplicationBuilder builder)
  {
    var app = builder.Build();

    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
      endpoints.MapGrpcService<OrganizationService>();

      endpoints.MapGet("/",
        async context =>
        {
          await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        });
    });


    return app.RunAsync();
  }

  private static void StartupBanner(this WebApplicationBuilder builder)
  {
    const string divider = "------------------------------------------------------------------";
    const ConsoleColor dividerColor = ConsoleColor.DarkMagenta;

    // Product Name
    Console.ForegroundColor = dividerColor;
    Console.WriteLine($@"
{divider}
{ProductName}
{divider}");
    Console.ResetColor();

    // Information
    var urls = builder.WebHost
      .GetSetting(WebHostDefaults.ServerUrlsKey)?
      .Replace(";", " ");
    ConsoleMessage("Urls", $"{urls}", ConsoleColor.DarkCyan);
    ConsoleMessage("Version", Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "");
    ConsoleMessage("Runtime", $"{RuntimeInformation.FrameworkDescription} - {builder.Environment.EnvironmentName}");
    ConsoleMessage("Architecture", RuntimeInformation.ProcessArchitecture.ToString());
    ConsoleMessage("Platform", RuntimeInformation.OSDescription);

    Console.ForegroundColor = dividerColor;
    Console.WriteLine(divider);
    Console.ResetColor();
  }

  private static void ConsoleMessage(string title, string value, ConsoleColor? color = null, int titleWidth = 15)
  {
    var label = $"{title.PadLeft(titleWidth)}: ";
    Console.Write(label);

    if (color == null)
    {
      Console.WriteLine(value);
    }
    else
    {
      Console.ForegroundColor = color.Value;
      Console.WriteLine(value, color.Value);
      Console.ResetColor();
    }
  }

  private static string ProductName
  {
    get
    {
      var assembly = Assembly.GetEntryAssembly();
      if (assembly == null)
        return string.Empty;

      var customAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
      return customAttributes is { Length: > 0 } ? ((AssemblyProductAttribute)customAttributes[0]).Product : string.Empty;
    }
  }
}
