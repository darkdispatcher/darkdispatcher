using System.Collections.Generic;
using DarkDispatcher.Domain.SeedWork;
using Xunit;

namespace DarkDispatcher.Domain.Tests.SeedWork;

public class EnumerationTests
{
  [Theory]
  [MemberData(nameof(NameData))]
  public void GivenTestColor_WhenToString_ShouldReturnName(TestColor color)
  {
    // Arrange

    // Act
    var result = color.ToString();

    // Assert
    Assert.Equal(color.Name, result);
  }

  [Fact]
  public void GivenTestColor_WhenGetAll_ShouldReturnAllColors()
  {
    // Arrange
    var expected = new List<TestColor>
    {
      TestColor.Red,
      TestColor.Green,
      TestColor.Blue
    };

    // Act
    var colors = Enumeration.GetAll<TestColor>();

    // Assert
    Assert.Equal(expected, colors);
  }

  [Fact]
  public void GivenTestColor_WhenCompareTo_ShouldInvalid()
  {
    // Arrange
    var color1 = TestColor.Red;
    var color2 = TestColor.Green;
    const int expected = -1;
    
    // Act
    var result = color1.CompareTo(color2);
    
    // Assert
    Assert.Equal(expected, result);
  }
  
  [Fact]
  public void GivenTestColor_WhenCompareTo_ShouldValid()
  {
    // Arrange
    var color1 = TestColor.Red;
    var color2 = TestColor.Red;
    const int expected = 0;
    
    // Act
    var result = color1.CompareTo(color2);
    
    // Assert
    Assert.Equal(expected, result);
  }

  [Fact]
  public void GivenTestColor_WhenWherePredicate_ShouldFilterList()
  {
    // Arrange
    var expected = new List<TestColor>
    {
      TestColor.Red,
      TestColor.Green
    };
    
    // Act
    var colors = Enumeration.Where<TestColor>(c => c.HexValue.StartsWith("#9900") || c.HexValue.StartsWith("#0099") );
    
    // Assert
    Assert.Equal(expected, colors);
  }
  
  [Fact]
  public void GivenTestColor_WhenSingleOrDefault_ShouldResultInSingle()
  {
    // Arrange
    var expected = TestColor.Red;
    
    // Act
    var color = Enumeration.SingleOrDefault<TestColor>(c => c.HexValue.StartsWith("#9900"));
    
    // Assert
    Assert.Equal(expected, color);
  }
  
  [Fact]
  public void GivenTestColor_WhenSingleOrDefault_ShouldResultInDefault()
  {
    // Arrange
    var expected = default(TestColor);
    
    
    // Act
    var color = Enumeration.SingleOrDefault<TestColor>(c => c.HexValue.StartsWith("#8888"));
    
    // Assert
    Assert.Equal(expected, color);
  }

  public record TestColor(int Id, string Name, string HexValue) : Enumeration(Id, Name)
  {
    public static readonly TestColor Red = new(1, "Red", "#990000");
    public static readonly TestColor Green = new(2, "Green", "#009900");
    public static readonly TestColor Blue = new(3, "Blue", "#000099");
  }
  
  public static TheoryData<TestColor> NameData => new()
  {
    TestColor.Red,
    TestColor.Green,
    TestColor.Blue
  };
}