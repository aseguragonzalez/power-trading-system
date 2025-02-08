using FluentAssertions;
using TradingSystem.Infrastructure.Ports;

namespace TradingSystem.Infrastructure.UnitTests.Ports;

public sealed class TradingSystemAppSettingsTest
{
    [Fact]
    public void ShouldFailsWhenTimeZoneIdIsMissing()
    {
        // Arrange
        Action act = () => _ = new TradingSystemAppSettings(timeZoneId: null!);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'timeZoneId')");
    }

    [Fact]
    public void ShouldFailsWhenTimeZoneIdIsInvalid()
    {
        // Arrange
        Action act = () => _ = new TradingSystemAppSettings(timeZoneId: "InvalidTimeZoneId");

        // Act & Assert
        act.Should().Throw<ArgumentException>().WithMessage("TimeZoneId (InvalidTimeZoneId) is invalid");
    }

    [Fact]
    public void ShouldFailsWhenTimeBewteenReportsInSecondsIsZero()
    {
        // Arrange
        Action act = () => _ = new TradingSystemAppSettings(timeBewteenReportsInSeconds: -1);

        // Act & Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Arrange
        string timeZoneId = "Central Africa Time";
        int timeBewteenReportsInSeconds = 1;
        TradingSystemAppSettings tradingSystemAppSettings = new(timeZoneId: timeZoneId, timeBewteenReportsInSeconds: timeBewteenReportsInSeconds);

        // Assert
        tradingSystemAppSettings.Should().NotBeNull();
        tradingSystemAppSettings.TimeZoneId.Should().Be(timeZoneId);
        tradingSystemAppSettings.TimeBewteenReportsInSeconds.Should().Be(timeBewteenReportsInSeconds);
    }

    [Fact]
    public void ShouldCreateAnInstanceWithDefaultValues()
    {
        // Arrange
        TradingSystemAppSettings tradingSystemAppSettings = new();

        // Assert
        tradingSystemAppSettings.Should().NotBeNull();
        tradingSystemAppSettings.TimeZoneId.Should().Be(TradingSystemAppSettings.DefaultTimeZoneId);
        tradingSystemAppSettings.TimeBewteenReportsInSeconds.Should().Be(TradingSystemAppSettings.DefaultTimeBewteenReports);
    }
}
