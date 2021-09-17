using System;
using System.Linq;
using DarkDispatcher.Core.Exceptions;
using DarkDispatcher.Core.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DarkDispatcher.Core.Tests.Domain
{
  public class AggregateTests
  {
    [Fact]
    public void GivenANewAggregate_WhenApplyingAnEvent_ShouldAddItToUncommittedEvents()
    {
      // Arrange
      var id = new TestAggregateId(Guid.NewGuid().ToString());
      var aggregate = new TestAggregate(id, "test");
      aggregate.ClearChanges(); // Clear new event

      // Act
      aggregate.Update("test2");

      // Assert
      var @event = aggregate.Changes.Single();
      @event.Should().BeOfType<TestEvents.TestAggregateUpdated>();
    }

    [Fact]
    public void GivenANewAggregate_WhenClearingChanges_ShouldHaveEmptyChanges()
    {
      // Arrange
      var id = new TestAggregateId(Guid.NewGuid().ToString());
      var aggregate = new TestAggregate(id, "test");

      // Act
      aggregate.Update("test2");
      aggregate.Delete();
      aggregate.ClearChanges();

      // Assert
      aggregate.Changes.Should().BeEmpty();
    }

    [Fact]
    public void GivenLoadFromHistory_WhenPassingAnInvalidEvent_ShouldThrowAnException()
    {
      // Arrange
      var aggregate = new TestAggregate();

      // Act
      Assert.Throws<NoEventHandlerRegisteredException<TestAggregateState>>(() =>
        aggregate.Load(new[] { new TestEvents.Invalid("test") }));

      // Assert
    }
  }
}