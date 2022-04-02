using DarkDispatcher.Application.Common.Behaviors;
using DarkDispatcher.Application.Modules.Accounts.Commands;
using DarkDispatcher.Core;
using DarkDispatcher.Infrastructure.Marten;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weasel.Core;

namespace DarkDispatcher.Infrastructure;

public static class Extensions
{
  public static IDarkDispatcherBuilder AddInfrastructure(this IDarkDispatcherBuilder builder, IHostEnvironment environment)
  {
    builder.Services
      .AddHttpContextAccessor()
      .AddMediatR(typeof(CreateOrganization).Assembly)
      .AddHostedService<SeedService>();

    builder
      .AddValidations()
      .AddMarten(options =>
      {
        // Use the more permissive schema auto create behavior while in development
        if (environment.IsDevelopment())
        {
          options.AutoCreateSchemaObjects = AutoCreate.All;
        }
      });

    return builder;
  }
}
