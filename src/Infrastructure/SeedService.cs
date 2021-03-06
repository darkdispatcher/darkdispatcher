using System;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Application.Modules.Accounts.Commands;
using DarkDispatcher.Application.Modules.Projects.Commands;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Domain.Features.Entities;
using DarkDispatcher.Domain.Projects.Entities;
using Marten;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DarkDispatcher.Infrastructure;

internal class SeedService : IHostedService
{
  private readonly ILogger<SeedService> _logger;
  private readonly IServiceProvider _services;

  public SeedService(
    ILogger<SeedService> logger,
    IServiceProvider services)
  {
    _logger = logger;
    _services = services;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    using var scope = _services.CreateScope();
    var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    var idGenerator = scope.ServiceProvider.GetRequiredService<IIdGenerator>();

    if (environment.IsDevelopment())
    {
      var documentStore = scope.ServiceProvider.GetRequiredService<IDocumentStore>();
      await documentStore.Advanced.Clean.CompletelyRemoveAllAsync();
    }

    // Organization
    _logger.LogInformation("Seeding organizations...");
    var organization = await mediator.Send(new CreateOrganization.Command("Acme"), cancellationToken);

    // Project
    _logger.LogInformation("Seeding projects...");
    var project = await mediator.Send(new CreateProject.Command(organization.GetAggregateId(), "Demo", "Demo project"), cancellationToken);

    // Tags
    _logger.LogInformation("Seeding tags...");
    var tag = new Tag(idGenerator.New(), "demo", TagColor.Concrete);
    project.CreateTag(tag);

    _logger.LogInformation("Updating project with tags...");
    await mediator.Send(new UpdateProject.Command(project), cancellationToken);

    // Environments
    _logger.LogInformation("Seeding environments...");
    var development = await mediator.Send(new CreateEnvironment.Command(project.GetAggregateId(), "Development", "Development Environment", EnvironmentColor.FireEngineRed), cancellationToken);
    var staging = await mediator.Send(new CreateEnvironment.Command(project.GetAggregateId(), "Staging", "Staging Environment", EnvironmentColor.WindsorTan), cancellationToken);
    var production = await mediator.Send(new CreateEnvironment.Command(project.GetAggregateId(), "Production", "Production Environment", EnvironmentColor.GreenPigment), cancellationToken);

    // Configuration
    _logger.LogInformation("Seeding configuration...");
    var configuration = await mediator.Send(new CreateConfiguration.Command(project.GetAggregateId(), "Demo Config", "Demo configuration"), cancellationToken);
    configuration.Update("demo-config", "Demo Config");
    configuration = await mediator.Send(new UpdateConfiguration.Command(configuration), cancellationToken);

    // Features
    _logger.LogInformation("Seeding features...");
    var trueVariation = new Variation(idGenerator.New(), "true");
    var falseVariation = new Variation(idGenerator.New(), "false");
    var variations = new[]
    {
      trueVariation,
      falseVariation
    };
    var defaults = new RuleVariationDefaults(trueVariation.Id, falseVariation.Id);
    var feature = await mediator.Send(new CreateFeature.Command(configuration.GetAggregateId(), "feature1", "Demo Feature", VariationType.Boolean, variations, defaults, Description: "Demo feature to test"), cancellationToken);
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
