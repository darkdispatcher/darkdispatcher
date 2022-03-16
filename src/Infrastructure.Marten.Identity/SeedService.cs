using System;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Ids;
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

    const string adminRoleName = "admin";
    var role = await roleManager.FindByNameAsync(adminRoleName);
    if (role == null)
    {
      role = new Role {  Id = idGenerator.New(), Name = adminRoleName };
      await roleManager.CreateAsync(role);
    }

    var user = await userManager.FindByEmailAsync("joe@gmail.com");
    if (user == null)
    {
      user = new User("admin")
      {
        Id = idGenerator.New(),
        Email = "joe@gmail.com"
      };

      await userManager.CreateAsync(user);
      var password = Guid.NewGuid().ToString();
      await userManager.AddPasswordAsync(user, password);
      await userManager.AddToRoleAsync(user, adminRoleName);
    }
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
