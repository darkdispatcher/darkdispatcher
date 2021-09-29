using System;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Application.Features.Accounts.Commands;
using DarkDispatcher.Application.Features.Projects.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DarkDispatcher.Infrastructure
{
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

      var organization = await mediator.Send(new CreateOrganization.Command("Acme"), cancellationToken);
      var organizationId = organization.State.Id;
      var project = await mediator.Send(new CreateProject.Command(organizationId, "Demo", "Demo project"), cancellationToken);
      project.AddEnvironment(organizationId, "Development", "Development Environment");
      project.AddEnvironment(organizationId, "Staging", "Staging Environment");
      project.AddEnvironment(organizationId, "Production", "Production Environment");
      await mediator.Send(new UpdateProject.Command(project), cancellationToken);

      //await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }
  }
}