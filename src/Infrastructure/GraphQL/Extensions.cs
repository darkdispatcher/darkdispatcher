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
      var graphqlBuilder = builder.Services
        .AddGraphQLServer()
        .AddAuthorization();

      graphqlBuilder
        .AddQueryType()
        .AddOrganizations();

      return builder;
    }

    public static IEndpointRouteBuilder UseGraphQLServer(this IEndpointRouteBuilder builder)
    {
      builder
        .MapGraphQLHttp("/graphql")
        .WithOptions(new GraphQLHttpOptions
        {
          EnableGetRequests = true,
          AllowedGetOperations = AllowedGetOperations.QueryAndMutation
        });

      builder
        .MapBananaCakePop("/explorer")
        .WithOptions(new GraphQLToolOptions
        {
          Enable = true,
          Title = "Dark Dispatcher Api Explorer",
          Document = "",
          GraphQLEndpoint = "/graphql",
          UseBrowserUrlAsGraphQLEndpoint = true,
          DisableTelemetry = true,
          HttpMethod = DefaultHttpMethod.Get
        });

      builder
        .MapGraphQLSchema("/schema");

      return builder;
    }
  }
}