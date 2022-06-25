using DarkDispatcher.Core;
using DarkDispatcher.Server.Api.Organizations;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Server.Config;

public static class GraphQlServices
{
  /// <summary>
  /// Add GraphQL Services to Server
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  public static IDarkDispatcherBuilder AddDarkDispatcherGraphQl(this IDarkDispatcherBuilder builder)
  {
    builder.Services
      .AddGraphQLServer()
      .ModifyOptions(options =>
      {
        options.UseXmlDocumentation = true;
      })
      .AddAuthorization()

      // Next we add the types to our schema.
      .AddQueryType()
      .AddMutationType()
      //.AddSubscriptionType()
      .AddTypeExtension<OrganizationQueries>()
      .AddTypeExtension<OrganizationMutations>()
      .AddType<Organization>();

    return builder;
  }
}
