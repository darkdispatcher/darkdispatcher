using DarkDispatcher.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Infrastructure.Marten.Identity;

public static class Extensions
{
  public static IDarkDispatcherBuilder AddMartenIdentity(this IDarkDispatcherBuilder builder)
  {
    builder.Services
      .AddIdentityCore<User>(options =>
      {

      })
      .AddRoles<Role>()
      .AddUserStore<MartenUserStore>()
      .AddRoleStore<MartenRoleStore>();


    builder.Services.AddHostedService<SeedService>();

    return builder;
  }
}
