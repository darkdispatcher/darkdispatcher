using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DarkDispatcher.Core;
using DarkDispatcher.Core.Persistence;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Services;
using Marten.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Weasel.Core;
using Weasel.Postgresql;
using IEventStore = DarkDispatcher.Core.Persistence.IEventStore;
using IProjection = DarkDispatcher.Core.Domain.IProjection;

namespace DarkDispatcher.Infrastructure.Marten
{
  internal static class Extensions
  {
    private const string DefaultConfigKey = "EventStore";

    public static IDarkDispatcherBuilder AddMarten(
      this IDarkDispatcherBuilder builder,
      IConfiguration configuration,
      Action<StoreOptions>? configureOptions = null)
    {
      builder.Services.TryAddTransient<IEventStore, MartenEventStore>();
      builder.Services.TryAddScoped<IReadRepository, MartenReadRepository>();
      
      var martenConfig = new MartenOptions();
      configuration.GetSection(DefaultConfigKey).Bind(martenConfig);

      var documentStore = builder.Services
        .AddMarten(options =>
        {
          options.Connection(martenConfig.ConnectionString);
          options.AutoCreateSchemaObjects = AutoCreate.All;
          options.Events.DatabaseSchemaName = martenConfig.WriteModelSchema;
          options.DatabaseSchemaName = martenConfig.ReadModelSchema;
          options.Serializer(new SystemTextJsonSerializer
          {
            Casing = Casing.Default,
            EnumStorage = EnumStorage.AsString
          });
          
          options.Events.StreamIdentity = StreamIdentity.AsString;
          options.Events.TenancyStyle = TenancyStyle.Conjoined;

          //options.Advanced.DefaultTenantUsageEnabled = false;

          // Projections
          options.Projections.AsyncMode = martenConfig.DaemonMode;
          options.Projections.RegisterAllProjections();
          
          options.Policies.AllDocumentsAreMultiTenanted();
          
          configureOptions?.Invoke(options);
        })
        .InitializeStore();

      if (martenConfig.ShouldRecreateDatabase)
        documentStore.Advanced.Clean.CompletelyRemoveAll();

      // TODO: Move to Hosted Service?
      documentStore.Schema.ApplyAllConfiguredChangesToDatabaseAsync().Wait();

      return builder;
    }

    /// <summary>
    /// Registers all projections of <see cref="IProjection"/>
    /// </summary>
    /// <param name="options">The <see cref="ProjectionOptions"/></param>
    /// <param name="lifecycle">The <see cref="ProjectionLifecycle"/></param>
    /// <returns><see cref="ProjectionOptions"/></returns>
    public static ProjectionOptions RegisterAllProjections(this ProjectionOptions options, ProjectionLifecycle lifecycle = ProjectionLifecycle.Async)
    {
      var projections = GetAllProjections();
      var methodInfo = typeof(ProjectionOptions).GetMethod(nameof(ProjectionOptions.SelfAggregate), new[] { typeof(ProjectionLifecycle?) });

      foreach (var view in projections)
      {
        var selfAggregateMethod = methodInfo!.MakeGenericMethod(view);
        var parameters = new object[] { lifecycle };
        selfAggregateMethod.Invoke(options, parameters);
      }

      return options;
    }

    /// <summary>
    /// Get all projections types (<see cref="IProjection"/>) in Dark Dispatcher assemblies
    /// </summary>
    /// <returns>Types that inherit from <see cref="IProjection"/></returns>
    public static IReadOnlyCollection<Type> GetAllProjections()
    {
      var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName!.StartsWith("DarkDispatcher"));
      var types = assemblies.SelectMany(x => x.GetTypes())
        .Where(x => typeof(IProjection).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract && !x.IsGenericTypeDefinition)
        .ToImmutableList();
      return types;
    }
  }
}