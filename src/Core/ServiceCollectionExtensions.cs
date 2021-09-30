using DarkDispatcher.Core.Commands;
using DarkDispatcher.Core.Events;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Core.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DarkDispatcher.Core
{
  internal static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
      where TEvent : class, IDomainEvent
      where TEventHandler : class, IEventHandler<TEvent>
    {
      return services
        .AddTransient<TEventHandler>()
        .AddTransient<INotificationHandler<TEvent>>(sp => sp.GetRequiredService<TEventHandler>())
        .AddTransient<IEventHandler<TEvent>>(sp => sp.GetRequiredService<TEventHandler>());
    }
    
    public static IDarkDispatcherBuilder AddDarkDispatcherCore(this IServiceCollection services)
    {
      services
        .AddMediatR()
        .AddScoped<ICommandBus, CommandBus>()
        .AddScoped<IQueryBus, QueryBus>();
      
      services.AddScoped<IAggregateStore, AggregateStore>();
      services.TryAddScoped<IIdGenerator, GuidIdGenerator>();
      services.TryAddScoped<IEventStore, InMemoryEventStore>();
      services.TryAddScoped<IEventBus, EventBus>();
      
      return DarkDispatcherBuilder.Create(services);
    }
    
    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
      return services.AddScoped<IMediator, Mediator>()
        .AddTransient<ServiceFactory>(sp => sp.GetRequiredService);
    }
  }
}