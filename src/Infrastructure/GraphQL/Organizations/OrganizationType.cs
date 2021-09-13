using HotChocolate;

namespace DarkDispatcher.Infrastructure.GraphQL.Organizations
{
  [GraphQLName("Organization")]
  public class OrganizationType
  {
    public string Name { get; set; }
  }
}