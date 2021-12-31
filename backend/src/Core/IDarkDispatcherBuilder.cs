using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Core;

public interface IDarkDispatcherBuilder
{
  IConfiguration Configuration { get; }
  IServiceCollection Services { get; }
}

internal class DarkDispatcherBuilder : IDarkDispatcherBuilder
{
  private readonly IConfiguration _configuration;
  private readonly IServiceCollection _services;

  IConfiguration IDarkDispatcherBuilder.Configuration => _configuration;
  IServiceCollection IDarkDispatcherBuilder.Services => _services;

  private DarkDispatcherBuilder(IConfiguration configuration, IServiceCollection services)
  {
    _configuration = configuration;
    _services = services;
  }
    
  public static IDarkDispatcherBuilder Create(IConfiguration configuration, IServiceCollection services) => new DarkDispatcherBuilder(configuration, services);
}