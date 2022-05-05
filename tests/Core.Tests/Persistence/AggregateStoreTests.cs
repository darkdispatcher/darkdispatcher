using System.Threading.Tasks;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Core.Tests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace DarkDispatcher.Core.Tests.Persistence;

public class AggregateStoreTests
{
  [Fact]
  public async Task GivenAggregateStore_WhenStoreIsEmpty_ShouldBeNull()
  {
    // Arrange
    var eventStore = new Mock<IEventStore>();
    var sut = new AggregateStore(eventStore.Object);
    var id = new TestAggregateId("1");

    // Act
    var result = await sut.LoadAsync<TestAggregate>(id);

    // Assert
    result.Should().BeNull();
  }
}
