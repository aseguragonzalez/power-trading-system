using FluentAssertions;
using TradingSystem.Infrastructure.Adapters.Services;

namespace TradingSystem.UnitTests.Infrastructure.Adapters.Services;

public class ResilientTradeServiceSettingsTest
{
    [Fact]
    public void ShouldFailsWhenDelayBetweenRetriesIsNegative()
    {
        // Arrange
        Action act = () => _ = new ResilientTradeServiceSettings(secondsBetweenRetries: -1);

        // Act and Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Act
        ResilientTradeServiceSettings settings = new(secondsBetweenRetries: 0);

        // Assert
        settings.Should().NotBeNull();
    }
}
