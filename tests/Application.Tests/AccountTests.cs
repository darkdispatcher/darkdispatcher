using System.Linq;
using DarkDispatcher.Domain.Accounts;
using DarkDispatcher.Domain.Accounts.Events.v1;
using DarkDispatcher.Domain.Accounts.Ids;
using FluentAssertions;
using Xunit;

namespace DarkDispatcher.Application.Tests;

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
    @event.Should().BeOfType<OrganizationCreated>();
  }
}
