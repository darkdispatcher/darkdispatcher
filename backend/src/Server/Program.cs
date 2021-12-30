using DarkDispatcher.Core;
using DarkDispatcher.Infrastructure;
using DarkDispatcher.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
  // Setup a HTTP/2 endpoint without TLS.
  options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services
  .AddDarkDispatcherCore()
  .AddInfrastructure(builder.Configuration, builder.Environment);
      
builder.Services.AddGrpc(options =>
{
  options.EnableDetailedErrors = true;
});
builder.Services.AddGrpcReflection();


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
        
  if (app.Environment.IsDevelopment())
  {
    endpoints.MapGrpcReflectionService();
  }
});

app.Run();

// public class Program
// {
//   public static void Main(string[] args)
//   {
//     CreateHostBuilder(args).Build().Run();
//   }
//
//   // Additional configuration is required to successfully run gRPC on macOS.
//   // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
//   public static IHostBuilder CreateHostBuilder(string[] args) =>
//     Host.CreateDefaultBuilder(args)
//       .ConfigureWebHostDefaults(webBuilder =>
//       {
//         webBuilder.ConfigureKestrel(options =>
//         {
//           options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
//         });
//         webBuilder.UseStartup<Startup>();
//       });
// }