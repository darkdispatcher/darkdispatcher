using System.Reflection;
using DarkDispatcher.Core.Extensions;
using DarkDispatcher.Core.Tests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace DarkDispatcher.Core.Tests.Extensions;

public class AssemblyExtensionsTests
{
  [Fact]
  public void GivenAnAssembly_WhenInvokingGetVersion_ShouldGetCorrectVersion()
  {
    // Arrange
    const string expect = "1.0.1.0-tests";
    var assembly = typeof(InvalidAggregate).Assembly;

    // Act
    var result = assembly.GetInformationalVersion();

    // Assert
    result.Should().StartWith(expect);
  }

  [Fact]
  public void GivenANullAssembly_WhenInvokingGetVersion_ShouldGetUnknown()
  {
    // Arrange
    var assembly = null as Assembly;

    // Act
    var result = assembly.GetInformationalVersion();

    // Assert
    result.Should().Be("unknown");
  }

  [Fact]
  public void GivenAnAssembly_WhenInvokingGetProduct_ShouldGetCorrectProduct()
  {
    // Arrange
    var assembly = typeof(InvalidAggregate).Assembly;

    // Act
    var result = assembly.GetProductName();

    // Assert
    result.Should().Be("Core Tests Helpers");
  }

  [Fact]
  public void GivenAnInvalidAssembly_WhenInvokingGetProduct_ShouldBeEmpty()
  {
    // Arrange
    var assembly = null as Assembly;

    // Act
    var result = assembly.GetProductName();

    // Assert
    result.Should().BeEmpty();
  }
}
