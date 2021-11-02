using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Core
{
  internal class DarkDispatcherBuilder : IDarkDispatcherBuilder
  {
    private readonly IServiceCollection _services;
    IServiceCollection IDarkDispatcherBuilder.Services => _services;

    private DarkDispatcherBuilder(IServiceCollection services)
    {
      _services = services;
    }
    
    public static IDarkDispatcherBuilder Create(IServiceCollection services) => new DarkDispatcherBuilder(services);
  }
}