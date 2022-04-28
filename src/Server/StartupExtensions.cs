using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DarkDispatcher.Core;
using DarkDispatcher.Infrastructure;
using DarkDispatcher.Infrastructure.Logging;
using DarkDispatcher.Infrastructure.Marten.Identity;
using DarkDispatcher.Server.Queries;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Organization = DarkDispatcher.Server.Models.Organization;

namespace DarkDispatcher.Server;

public static class StartupExtensions
{
  public static WebApplicationBuilder AddDarkDispatcher(this WebApplicationBuilder builder)
  {
    builder.Logging.AddDefaultLogging(builder.Configuration);

    // Services
    builder.Services
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters =
          new TokenValidationParameters
          {
            ValidIssuer = "",
            ValidAudience = ""
          };
      });

    builder.Services
      .AddAuthorization()
      .AddDarkDispatcherCore(builder.Configuration)
      .AddInfrastructure(builder.Environment)
      .AddMartenIdentity();

    builder.Services
      .AddGraphQLServer()
      .AddAuthorization()

      // Next we add the types to our schema.
      .AddQueryType()
      .AddMutationType()
      .AddSubscriptionType()

      .AddTypeExtension<OrganizationQueries>()
      .AddType<Organization>();

    builder.StartupBanner();

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
        .RequireAuthorization()
        .WithOptions(new GraphQLServerOptions
        {
          AllowedGetOperations = AllowedGetOperations.QueryAndMutation,
          EnableSchemaRequests = true
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
