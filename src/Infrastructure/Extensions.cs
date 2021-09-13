using DarkDispatcher.Application.Common.Behaviors;
using DarkDispatcher.Application.Features.Accounts.Commands;
using DarkDispatcher.Core;
using DarkDispatcher.Domain.Accounts;
using DarkDispatcher.Domain.Accounts.Events;
using DarkDispatcher.Infrastructure.GraphQL;
using DarkDispatcher.Infrastructure.Marten;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weasel.Postgresql;

namespace DarkDispatcher.Infrastructure
{
  public static class Extensions
  {
    public static IDarkDispatcherBuilder AddInfrastructure(this IDarkDispatcherBuilder builder, IConfiguration configuration, IHostEnvironment environment)
    {
      builder.Services.AddHttpContextAccessor();
      builder.Services.AddMediatR(typeof(CreateOrganization).Assembly);
      
      builder.AddValidations();
      builder
        .AddMarten(configuration, options =>
        {
          // Use the more permissive schema auto create behavior while in development
          if (environment.IsDevelopment())
          {
            options.AutoCreateSchemaObjects = AutoCreate.All;
          }
        })
        .AddGraphQLServer();
      
      return builder;
    }
  }
}