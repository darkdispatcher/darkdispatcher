using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Infrastructure.GraphQL.Organizations
{
  public static class Extensions
  {
    public static IRequestExecutorBuilder AddOrganizations(this IRequestExecutorBuilder builder)
    {
      builder.AddTypeExtension<OrganizationQueries>();
      builder.AddType<Organization>();

      return builder;
    }
  }
}