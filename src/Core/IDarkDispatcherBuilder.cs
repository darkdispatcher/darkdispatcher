using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Core;

public interface IDarkDispatcherBuilder
{
  string Environment { get; }
  IConfiguration Configuration { get; }
  IServiceCollection Services { get; }
}

internal class DarkDispatcherBuilder : IDarkDispatcherBuilder
{
  private readonly string _environment;
  private readonly IConfiguration _configuration;
  private readonly IServiceCollection _services;

  string IDarkDispatcherBuilder.Environment => _environment;
  IConfiguration IDarkDispatcherBuilder.Configuration => _configuration;
  IServiceCollection IDarkDispatcherBuilder.Services => _services;

  private DarkDispatcherBuilder(IConfiguration configuration, IServiceCollection services, string environment)
  {
    _configuration = configuration;
    _services = services;
    _environment = environment;
  }

  public static IDarkDispatcherBuilder Create(IConfiguration configuration, IServiceCollection services, string environment) => new DarkDispatcherBuilder(configuration, services, environment);
}
