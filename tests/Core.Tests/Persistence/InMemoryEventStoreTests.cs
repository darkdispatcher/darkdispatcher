using System.Threading.Tasks;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Core.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DarkDispatcher.Core.Tests.Persistence
{
  public class InMemoryEventStoreTests
  {


    [Fact]
    public async Task GivenInMemoryStoreIsRegistered_WhenAddEvents_ShouldAddEventsToStore()
    {
      // Arrange
      var services = new ServiceCollection();
      services.AddScoped<IEventStore, InMemoryEventStore>();
      await using var provider = services.BuildServiceProvider();
      var eventStore = provider.GetService<IEventStore>();

      // Act
      var aggregate = new TestAggregate(new TestAggregateId("test1"), "Test1");
      aggregate.Delete();

      await eventStore.AddEventsAsync<TestAggregate>(aggregate.GetId(), aggregate.Version, aggregate.Changes);
      var events = await eventStore.GetEventsAsync(aggregate.GetId());
      
      // Assert
      events.Should().HaveCount(2);
      events.Should().ContainSingle(x => x is TestEvents.TestAggregateCreated);
      events.Should().ContainSingle(x => x is TestEvents.TestAggregateDeleted);
    }
  }
}