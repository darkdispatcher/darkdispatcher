using DarkDispatcher.Domain.ValueObjects;
using Xunit;

namespace DarkDispatcher.Domain.Tests.ValueObjects;

public class NameTests
{
  [Fact]
  public void GivenAName_WhenCallingToString_ThenShouldReturnConcatenated()
  {
    // Arrange
    var name = new Name("John", "Doe");
    const string expected = "John Doe";

    // Act
    var actual = name.ToString();

    // Assert
    Assert.Equal(expected, actual);
  }

  [Fact]
  public void GivenTwoNames_WhenComparing_ShouldEqual()
  {
    // Arrange
    var name1 = new Name("John", "Doe");
    var name2 = new Name("John", "Doe");

    // Act
    var result = name1.Equals(name2);

    // Assert
    Assert.True(result);
  }

  [Fact]
  public void GivenTwoNames_WhenComparing_ShouldNotEqual()
  {
    // Arrange
    var name1 = new Name("John", "Doe");
    var name2 = new Name("Jane", "Doe");

    // Act
    var result = name1.Equals(name2);

    // Assert
    Assert.False(result);
  }
}
