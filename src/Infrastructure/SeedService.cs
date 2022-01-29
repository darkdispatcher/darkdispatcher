using System;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Application.Modules.Accounts.Commands;
using DarkDispatcher.Application.Modules.Projects.Commands;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Domain.Features;
using DarkDispatcher.Domain.Features.Entities;
using DarkDispatcher.Domain.Projects;
using DarkDispatcher.Domain.Projects.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Environment = DarkDispatcher.Domain.Projects.Entities.Environment;

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
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    var idGenerator = scope.ServiceProvider.GetRequiredService<IIdGenerator>();

    // Organization
    var organization = await mediator.Send(new CreateOrganization.Command("Acme"), cancellationToken);
      
    // Project
    var project = await mediator.Send(new CreateProject.Command(organization.GetAggregateId(), "Demo", "Demo project"), cancellationToken);
      
    // Environments
    var development = new Environment(idGenerator.New(), "Development", "Development Environment", EnvironmentColor.FireEngineRed);
    var staging = new Environment(idGenerator.New(), "Staging", "Staging Environment", EnvironmentColor.WindsorTan);
    var production = new Environment(idGenerator.New(), "Production", "Production Environment", EnvironmentColor.GreenPigment);
    project.CreateEnvironment(development);
    project.CreateEnvironment(staging);
    project.CreateEnvironment(production);

    // Tags
    var tag = new Tag(idGenerator.New(), "demo", TagColor.Concrete);
    project.CreateTag(tag);

    await mediator.Send(new UpdateProject.Command(project), cancellationToken);

    // Configuration
    var configuration = await mediator.Send(new CreateConfiguration.Command(project.GetAggregateId(), "Demo Config", "Demo configuration"), cancellationToken);
    configuration.Update("demo-config", "Demo Config");
    configuration = await mediator.Send(new UpdateConfiguration.Command(configuration), cancellationToken);
      
    // Features
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