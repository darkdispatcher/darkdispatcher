using System.Linq;
using Xunit;
using DarkDispatcher.Domain.Accounts;
using FluentAssertions;

namespace DarkDispatcher.Application.Tests
{
  public class AccountTests
  {
    [Fact]
    public void GivenANewOrganization_WhenCheckingTheChanges_ShouldHaveRaisedCreatedEvent()
    {
      // Given
      var id = new OrganizationId("acme");

      // When
      var organization = new Organization(id, "Acme");

      // Then
      var @event = organization.Changes.Single();
      @event.Should().BeOfType<AccountEvents.V1.OrganizationCreated>();
    }
  }
}