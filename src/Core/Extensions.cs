using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DarkDispatcher.Core
{
  public static class Extensions
  {
    public static IDarkDispatcherBuilder AddDarkDispatcher(this IServiceCollection services)
    {
      var builder = DarkDispatcherBuilder.Create(services);

      builder.Services.AddScoped<IAggregateStore, AggregateStore>();
      builder.Services.TryAddScoped<IIdGenerator, GuidIdGenerator>();
      builder.Services.TryAddScoped<IEventStore, InMemoryEventStore>();
      
      return builder;
    }
  }
}