using System;
using DarkDispatcher.Core;
using DarkDispatcher.Core.Domain;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Accounts;
using DarkDispatcher.Domain.Projects;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Services;
using Marten.Storage;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using IEventStore = DarkDispatcher.Core.Persistence.IEventStore;
using IProjection = Marten.Events.Projections.IProjection;

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
      builder.Services.AddScoped<IEventStore, MartenEventStore>();
      builder.Services.AddScoped<IReadRepository, MartenReadRepository>();
      
      var martenConfig = new MartenOptions();
      configuration.GetSection(DefaultConfigKey).Bind(martenConfig);

      var documentStore = builder.Services
        .AddMarten(options =>
        {
          options.Connection(martenConfig.ConnectionString);
          options.DatabaseSchemaName = "dd";

          options.Events.StreamIdentity = StreamIdentity.AsString;
          options.Events.TenancyStyle = TenancyStyle.Conjoined;

          //options.Advanced.DefaultTenantUsageEnabled = false;

          // Projections
          options.Projections.
            

          options.Policies.AllDocumentsAreMultiTenanted();
          
          options.Serializer(new SystemTextJsonSerializer
          {
            Casing = Casing.Default,
            EnumStorage = EnumStorage.AsString
          });

          configureOptions?.Invoke(options);
        })
        .InitializeStore();

      if (martenConfig.ShouldRecreateDatabase)
        documentStore.Advanced.Clean.CompletelyRemoveAll();

      // TODO: Move to Hosted Service?
      documentStore.Schema.ApplyAllConfiguredChangesToDatabaseAsync().Wait();

      return builder;
    }
  }
}