namespace TradingSystem.Domain.UnitTests.Services;

using AutoFixture;
using FluentAssertions;
using TradingSystem.Domain.Services;

public class TradePositionTest
{
    [Fact]
    public void ShouldCreateAnInstanceOfTradePosition()
    {
        // Arrange
        var fixture = new Fixture();
        int randomPeriodId = fixture.Create<int>() % 24 + 1;

        // Act
        TradePosition tradePosition = new(periodId: randomPeriodId, volume: 1.0);

        // Assert
        tradePosition.PeriodId.Should().Be(randomPeriodId);
        tradePosition.Volume.Should().Be(1.0);
    }

    [Fact]
    public void ShouldFailWhenPeriodIsGreaterThanLast()
    {
        // Arrange
        var fixture = new Fixture();
        int periodId = fixture.Create<int>() + 25;
        Action act = () => _ = new TradePosition(periodId: periodId, volume: 1.0);

        // Act & Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ShouldFailWhenPeriodIsLessThanFirst()
    {
        // Arrange
        var fixture = new Fixture();
        int periodId = fixture.Create<int>() % 24 - 24;
        Action act = () => _ = new TradePosition(periodId: periodId, volume: 1.0);

        // Act & Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
