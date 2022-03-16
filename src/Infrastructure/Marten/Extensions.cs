using System;
using System.Text.Json.Serialization;
using DarkDispatcher.Application.Modules.Projects.Projections;
using DarkDispatcher.Core;
using DarkDispatcher.Core.Extensions;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Core.Projections;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Services;
using Marten.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using Weasel.Postgresql;
using IEventStore = DarkDispatcher.Core.Persistence.IEventStore;

namespace DarkDispatcher.Infrastructure.Marten;

internal static class Extensions
{
  private const string DefaultConfigKey = "EventStore";

  public static IDarkDispatcherBuilder AddMarten(
    this IDarkDispatcherBuilder builder,
    Action<StoreOptions>? configureOptions = null)
  {
    builder.Services.AddTransient<IEventStore, MartenEventStore>();
    builder.Services.AddScoped<IReadRepository, MartenReadRepository>();

    var martenConfig = new MartenOptions();
    builder.Configuration.GetSection(DefaultConfigKey).Bind(martenConfig);

    var documentStore = builder.Services
      .AddMarten(options =>
      {
        options.Connection(martenConfig.ConnectionString);
        options.AutoCreateSchemaObjects = AutoCreate.All;
        options.Events.DatabaseSchemaName = martenConfig.WriteModelSchema;
        options.DatabaseSchemaName = martenConfig.ReadModelSchema;
        var systemTextJsonSerializer = new SystemTextJsonSerializer
        {
          Casing = Casing.CamelCase,
          EnumStorage = EnumStorage.AsString
        };
        systemTextJsonSerializer.Customize(o =>
        {
          o.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        options.Serializer(systemTextJsonSerializer);

        options.Events.StreamIdentity = StreamIdentity.AsString;
        options.Events.TenancyStyle = TenancyStyle.Conjoined;

        //options.Advanced.DefaultTenantUsageEnabled = false;

        options.Schema.For<EnvironmentProjection>().Identity(x => x.Id);

        // Projections
        options.Projections.AsyncMode = martenConfig.DaemonMode;
        options.Projections.RegisterAllProjections();

        options.Policies.AllDocumentsAreMultiTenanted();

        configureOptions?.Invoke(options);
      })
      .InitializeStore();

    if (martenConfig.ShouldRecreateDatabase)
    {
      documentStore.Advanced.Clean.CompletelyRemoveAll();
    }

    // TODO: Move to Hosted Service?
    documentStore.Schema.ApplyAllConfiguredChangesToDatabaseAsync().Wait();

    return builder;
  }

  /// <summary>
  /// Registers all projections of <see cref="IProjection"/> for Marten.
  /// </summary>
  /// <param name="options">The <see cref="ProjectionOptions"/></param>
  /// <param name="lifecycle">The <see cref="ProjectionLifecycle"/></param>
  /// <returns><see cref="ProjectionOptions"/></returns>
  private static ProjectionOptions RegisterAllProjections(this ProjectionOptions options, ProjectionLifecycle lifecycle = ProjectionLifecycle.Async)
  {
    var projections = typeof(IProjection<>).GetAllTypesImplementingOpenGenericType();
    var methodInfo = typeof(ProjectionOptions).GetMethod(nameof(ProjectionOptions.SelfAggregate), new[] { typeof(ProjectionLifecycle?) });

    foreach (var view in projections)
    {
      var selfAggregateMethod = methodInfo!.MakeGenericMethod(view);
      var parameters = new object[] { lifecycle };
      selfAggregateMethod.Invoke(options, parameters);
    }

    return options;
  }
}
