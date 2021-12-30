// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using DarkDispatcher.Core;
// using DarkDispatcher.Infrastructure;
// using DarkDispatcher.Server.Services;
//
// namespace DarkDispatcher.Server;
//
// public class Startup
// {
//   private readonly IConfiguration _configuration;
//   private readonly IWebHostEnvironment _environment;
//
//   public Startup(IConfiguration configuration, IWebHostEnvironment environment)
//   {
//     _configuration = configuration;
//     _environment = environment;
//   }
//     
//   public void ConfigureServices(IServiceCollection services)
//   {
//     services.AddDarkDispatcherCore()
//       .AddInfrastructure(_configuration, _environment);
//       
//     services.AddGrpc(options =>
//     {
//       options.EnableDetailedErrors = true;
//     });
//     services.AddGrpcReflection();
//   }
//
//   public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//   {
//     if (env.IsDevelopment())
//     {
//       app.UseDeveloperExceptionPage();
//     }
//
//     app.UseRouting();
//
//     app.UseEndpoints(endpoints =>
//     {
//       endpoints.MapGrpcService<OrganizationService>();
//
//       endpoints.MapGet("/",
//         async context =>
//         {
//           await context.Response.WriteAsync(
//             "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
//         });
//         
//       if (env.IsDevelopment())
//       {
//         endpoints.MapGrpcReflectionService();
//       }
//     });
//   }
// }