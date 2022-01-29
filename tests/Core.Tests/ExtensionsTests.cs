using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace DarkDispatcher.Core.Tests;

public class ExtensionsTests
{
  public ExtensionsTests()
  {
  }

  [Fact]
  public void GivenAddDarkDispatcherIsCalled_ThenAllServicesAreRegistered()
  {
    // Arrange
    var services = new ServiceCollection();
    var configurationMock = new Mock<IConfiguration>();

    // Act
    services.AddDarkDispatcherCore(configurationMock.Object);

    // Assert
    using var provider = services.BuildServiceProvider();
    var eventStore = provider.GetService<IEventStore>();
    eventStore.Should().NotBeNull();
    eventStore.Should().BeOfType<InMemoryEventStore>();

    var aggregateStore = provider.GetService<IAggregateStore>();
    aggregateStore.Should().NotBeNull();
    aggregateStore.Should().BeOfType<AggregateStore>();

    var idGenerator = provider.GetService<IIdGenerator>();
    idGenerator.Should().NotBeNull();
    idGenerator.Should().BeOfType<GuidIdGenerator>();
  }
}
