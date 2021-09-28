using System;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Application.Features.Accounts.Commands;
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

      var org1 = await mediator.Send(new CreateOrganization.Command("Acme"), cancellationToken);
      var org2 = await mediator.Send(new CreateOrganization.Command("Dunder Mifflin"), cancellationToken);
      org2.Update("Dunder");
 
      await mediator.Send(new UpdateOrganization.Command(org2), cancellationToken);

      //await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }
  }
}