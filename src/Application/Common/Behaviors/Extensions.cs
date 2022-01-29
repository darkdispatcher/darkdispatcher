using DarkDispatcher.Application.Modules.Accounts.Commands;
using DarkDispatcher.Core;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Application.Common.Behaviors;

internal static class Extensions
{
  public static IDarkDispatcherBuilder AddValidations(this IDarkDispatcherBuilder builder)
  {
    builder.Services.AddValidatorsFromAssembly(typeof(CreateOrganization).Assembly, includeInternalTypes: true);
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    return builder;
  }
}
