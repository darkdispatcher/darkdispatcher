using DarkDispatcher.Core.Extensions;
using DarkDispatcher.Core.Projections;
using FluentAssertions;
using Xunit;

namespace DarkDispatcher.Core.Tests.Extensions;

public class TypeExtensionsTests
{
  [Fact]
  public void GivenAGenericInterface_WhenClassesImplementInterface_ShouldFindAllConcreteImplementations()
  {
    // Arrange

    // Act
    var types = typeof(IProjection<>).GetAllTypesImplementingOpenGenericType();

    // Assert
    types.Should().HaveCount(2);
  }
}
