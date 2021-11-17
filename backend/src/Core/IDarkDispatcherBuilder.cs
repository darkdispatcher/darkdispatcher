using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Core;

public interface IDarkDispatcherBuilder
{
  IServiceCollection Services { get; }
}