using DarkDispatcher.Core;
using DarkDispatcher.Infrastructure.GraphQL.Organizations;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Infrastructure.GraphQL
{
  public static class Extensions
  {
    internal static IDarkDispatcherBuilder AddGraphQLServer(this IDarkDispatcherBuilder builder)
    {
      var graphqlBuilder = builder.Services.AddGraphQLServer();

      graphqlBuilder
        .AddQueryType()
        .AddTypeExtension<OrganizationQueries>();
        //.AddTypeExtension<ProjectQueries>();

        graphqlBuilder
          .AddType<OrganizationType>();
        //.AddType<ProjectType>();

      return builder;
    }
    
    public static IEndpointRouteBuilder UseGraphQLServer(this IEndpointRouteBuilder builder)
    {
      builder
        .MapGraphQL()
        .WithOptions(new GraphQLServerOptions
        {
          Tool =
          {
            Enable = true,
            Document = "Query",
            HttpMethod = DefaultHttpMethod.Get
          },
          EnableGetRequests = true,
          AllowedGetOperations = AllowedGetOperations.QueryAndMutation
        });

      return builder;
    }
  }
}