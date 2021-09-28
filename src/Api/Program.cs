using DarkDispatcher.Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DarkDispatcher.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureLogging((context, builder) =>
        {
          builder.AddLogging(context.Configuration);
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.CaptureStartupErrors(true);
          webBuilder.UseDefaultServiceProvider(options =>
          {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
          });
          webBuilder.UseStartup<Startup>();
        });
  }
}