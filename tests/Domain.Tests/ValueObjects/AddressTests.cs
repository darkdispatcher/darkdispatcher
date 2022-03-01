using DarkDispatcher.Domain.ValueObjects;
using Xunit;

namespace DarkDispatcher.Domain.Tests.ValueObjects;

public class AddressTests
{
  [Fact]
  public void GivenAnAddress_WhenCallingToString_ThenShouldReturnAddressInline()
  {
    // Arrange
    var address = new Address("Address1", "City", "State", "PostalCode", "Country", "Address2");
    const string expected = "Address1\nAddress2\nCity, State PostalCode\nCountry";

    // Act
    var actual = address.ToString();

    // Assert
    Assert.Equal(expected, actual);
  }

  [Fact]
  public void GivenTwoAddresses_WhenComparing_ShouldEqual()
  {
    // Arrange
    var address1 = new Address("Address1", "City", "State", "PostalCode", "Country", "Address2");
    var address2 = new Address("Address1", "City", "State", "PostalCode", "Country", "Address2");

    // Act
    var result = address1.Equals(address2);

    // Assert
    Assert.True(result);
  }

  [Fact]
  public void GivenTwoAddresses_WhenComparing_ShouldNotEqual()
  {
    // Arrange
    var address1 = new Address("Address1", "City", "State", "PostalCode", "Country", "Address2");
    var address2 = new Address("Address1", "City", "State", "PostalCode", "Country2");

    // Act
    var result = address1.Equals(address2);

    // Assert
    Assert.False(result);
  }
}
