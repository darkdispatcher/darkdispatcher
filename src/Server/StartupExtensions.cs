using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DarkDispatcher.Core;
using DarkDispatcher.Core.Extensions;
using DarkDispatcher.Infrastructure;
using DarkDispatcher.Infrastructure.Logging;
using DarkDispatcher.Infrastructure.Marten.Identity;
using DarkDispatcher.Server.Config;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace DarkDispatcher.Server;

public static class StartupExtensions
{
  public static WebApplicationBuilder AddDarkDispatcher(this WebApplicationBuilder builder)
  {
    builder.Logging.ConfigureForDarkDispatcher(builder.Configuration);

    // Services
    builder.Services
      .AddDarkDispatcherCore(builder.Configuration, builder.Environment.EnvironmentName)
      .AddDarkDispatcherAuthentication()
      .AddDarkDispatcherGraphQl()
      .AddInfrastructure(builder.Environment)
      .AddMartenIdentity();

    builder.AddStartupBanner();

    return builder;
  }

  public static Task RunDarkDispatcherAsync(this WebApplicationBuilder builder)
  {
    var app = builder.Build();

    app.UseWebSockets();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapGraphQL()
        .WithOptions(new GraphQLServerOptions
        {
          AllowedGetOperations = AllowedGetOperations.QueryAndMutation,
          EnableSchemaRequests = true
        });
    });

    return app.RunAsync();
  }

  private static void AddStartupBanner(this WebApplicationBuilder builder)
  {
    const string divider = "------------------------------------------------------------------";
    const ConsoleColor dividerColor = ConsoleColor.DarkMagenta;
    var assembly = Assembly.GetEntryAssembly();
    var version = assembly.GetInformationalVersion();
    var productName = assembly.GetProductName();

    // Product Name
    Console.ForegroundColor = dividerColor;
    Console.WriteLine($@"
{divider}
{productName}
{divider}");
    Console.ResetColor();

    // Information
    var urls = builder.WebHost
      .GetSetting(WebHostDefaults.ServerUrlsKey)?
      .Replace(";", " ");
    ConsoleMessage("Urls", $"{urls}", ConsoleColor.DarkCyan);
    ConsoleMessage("Version", version);
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
}
