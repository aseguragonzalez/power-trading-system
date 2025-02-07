using FluentAssertions;
using TradingSystem.Infrastructure.Services;

namespace TradingSystem.Infrastructure.UnitTests.Services;

public class ResilientTradeServiceSettingsTest
{
    [Fact]
    public void ShouldFailsWhenDelayBetweenRetriesIsNegative()
    {
        // Arrange
        Action act = () => _ = new ResilientTradeServiceSettings(delayBetweenRetries: -1);

        // Act and Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Specified argument was out of the range of valid values. (Parameter 'delayBetweenRetries')");
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Act
        ResilientTradeServiceSettings settings = new(delayBetweenRetries: 0);

        // Assert
        settings.Should().NotBeNull();
    }
}
