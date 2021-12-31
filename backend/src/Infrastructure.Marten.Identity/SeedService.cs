using System;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Ids;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DarkDispatcher.Infrastructure.Marten.Identity;

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
    var idGenerator = scope.ServiceProvider.GetRequiredService<IIdGenerator>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<MartenUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<MartenRole>>();

    var role = await roleManager.CreateAsync(new MartenRole
    {
      Id = Guid.NewGuid(),
      Name = "admin"
    });

    var user = await userManager.CreateAsync(new MartenUser
    {
      Id = Guid.NewGuid(),
      Email = "joe@gmail.com",
      UserName = "jdoe"
    });
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}