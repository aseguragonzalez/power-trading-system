namespace TradingSystem.UnitTests.Domain.Services;

using FluentAssertions;
using TradingSystem.Domain.Services;


public class TradePositionsTest
{
    [Fact]
    public void ShouldCreateAnEmptyInstanceWhenPositionsIsNull()
    {
        // Arrange & Act
        TradePositions tradePositions = new(positions: null);

        // Assert
        tradePositions.Positions.Should().BeEmpty();
    }

    [Fact]
    public void ShouldCreateAnEmptyInstanceWhenPositionsAreEmptyArray()
    {
        // Arrange & Act
        TradePositions tradePositions = new(positions: []);

        // Assert
        tradePositions.Positions.Should().BeEmpty();
    }

    [Fact]
    public void ShouldCreateAnEmptyInstanceUsingParameterlessConstructor()
    {
        // Arrange & Act
        TradePositions tradePositions = new();

        // Assert
        tradePositions.Positions.Should().BeEmpty();
    }

    [Fact]
    public void ShouldCreateAnInstanceWithPositions()
    {
        // Arrange
        TradePosition tradePosition = new(periodId: 1, volume: 1.0);

        // Act
        TradePositions tradePositions = new(positions: [tradePosition]);

        // Assert
        tradePositions.Positions.Should().Contain(tradePosition);
        tradePositions.Positions.Should().HaveCount(1);
    }

}
