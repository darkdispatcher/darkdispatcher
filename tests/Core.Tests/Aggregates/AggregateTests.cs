using System;
using System.Linq;
using DarkDispatcher.Core.Events;
using DarkDispatcher.Core.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DarkDispatcher.Core.Tests.Aggregates;

public class AggregateTests
{
  [Fact]
  public void GivenANewAggregate_ShouldBeVersionOne()
  {
    // Arrange
    var id = new TestAggregateId(Guid.NewGuid().ToString());
    var aggregate = new TestAggregate(id, "test");

    // Assert
    aggregate.Version.Should().Be(1);
  }

  [Fact]
  public void GivenANewAggregate_WhenAddingAnEvent_ShouldBeVersionTwo()
  {
    // Arrange
    var id = new TestAggregateId(Guid.NewGuid().ToString());
    var aggregate = new TestAggregate(id, "test");

    // Act
    aggregate.Update("test2");

    // Assert
    aggregate.Version.Should().Be(2);
  }

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
  public void GivenAnInvalidAggregateState_WhenRegisteringEventTwice_ShouldThrowException()
  {
    // Arrange

    // Act/Assert
    Assert.Throws<InvalidOperationException>(() => new InvalidAggregateState());
  }

  [Fact]
  public void GivenANewAggregate_WhenLoadingFromHistory_ShouldMutateProperly()
  {
    // Arrange
    var id = new TestAggregateId(Guid.NewGuid().ToString());
    var aggregate = new TestAggregate(id, "test");

    // Act
    aggregate.ClearChanges();
    var created = new TestEvents.TestAggregateCreated(id.Value, "test99");
    var updated = new TestEvents.TestAggregateUpdated(id.Value, "test101");
    aggregate.Load(new DomainEvent[] { created, updated });

    // Assert
    aggregate.Changes.Should().BeEmpty();
    aggregate.State.Name.Should().Be("test101");
  }
}
