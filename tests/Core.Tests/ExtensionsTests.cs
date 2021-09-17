using System;
using System.Threading.Tasks;
using DarkDispatcher.Core;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Tests
{
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
      
      // Act
      services.AddDarkDispatcher();
      
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
}