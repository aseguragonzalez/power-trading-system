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
        Action act = () => _ = new TradingSystemAppSettings(secondsBetweenReports: -1);

        // Act & Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Arrange
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");
        TimeSpan secondsBewteenReports = TimeSpan.FromSeconds(1);
        TradingSystemAppSettings tradingSystemAppSettings = new(
            timeZoneId: timeZoneInfo.Id, secondsBetweenReports: secondsBewteenReports.Seconds
        );

        // Assert
        tradingSystemAppSettings.Should().NotBeNull();
        tradingSystemAppSettings.TimeZone.Should().Be(timeZoneInfo);
        tradingSystemAppSettings.SecondsBetweenReports.Should().Be(secondsBewteenReports);
    }

    [Fact]
    public void ShouldCreateAnInstanceWithDefaultValues()
    {
        // Arrange
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TradingSystemAppSettings.DefaultTimeZoneId);
        TimeSpan secondsBewteenReports = TimeSpan.FromSeconds(TradingSystemAppSettings.DefaultSecondsBetweenReports);
        TradingSystemAppSettings tradingSystemAppSettings = new();

        // Assert
        tradingSystemAppSettings.Should().NotBeNull();
        tradingSystemAppSettings.TimeZone.Should().Be(timeZoneInfo);
        tradingSystemAppSettings.SecondsBetweenReports.Should().Be(secondsBewteenReports);
    }
}
