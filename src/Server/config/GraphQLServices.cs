using DarkDispatcher.Core;
using DarkDispatcher.Server.Api.Organizations;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Server.Config;

public static class GraphQLServices
{
  public static IDarkDispatcherBuilder ConfigureGraphQL(this IDarkDispatcherBuilder builder)
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
