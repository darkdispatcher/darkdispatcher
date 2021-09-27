using DarkDispatcher.Core;
using DarkDispatcher.Infrastructure;
using DarkDispatcher.Infrastructure.GraphQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DarkDispatcher.Api
{
  public class Startup
  {
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
      _configuration = configuration;
      _environment = environment;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
      services
        .AddDarkDispatcher()
        .AddInfrastructure(_configuration, _environment);
     
      services.AddAuthentication();
      services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.UseGraphQLServer();
      });
    }
  }
}