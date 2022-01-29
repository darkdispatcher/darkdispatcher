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
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

    var roleId = idGenerator.New();
    await roleManager.CreateAsync(new Role
    {
      Id = roleId,
      Name = "admin"
    });

    var user = new User("admin")
    {
      Id = idGenerator.New(),
      Email = "joe@gmail.com"
    };
    await userManager.CreateAsync(user);
    await userManager.AddPasswordAsync(user, "Adm!n123");
    await userManager.AddToRoleAsync(user, "admin");
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}