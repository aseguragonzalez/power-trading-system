using FluentAssertions;
using TradingSystem.Infrastructure.Ports;

namespace TradingSystem.UnitTests.Infrastructure.Ports;

public class AppArgsTest
{
    [Fact]
    public void ShouldReturnTimeZoneIdWhenItUseArgs()
    {
        // Arrange & Act
        AppArgs appArgs = new([AppArgs.TimeZoneIdArg, "UTC"]);

        // Assert
        appArgs.TimeZoneId.Should().Be("UTC"); ;
        appArgs.SecondsBetweenReports.Should().BeNull();
        appArgs.RetrySeconds.Should().BeNull();
        appArgs.Path.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnTimeZoneIdWhenItUseShortArgs()
    {
        // Arrange & Act
        AppArgs appArgs = new([AppArgs.TimeZoneIdShortArg, "UTC"]);

        // Assert
        appArgs.TimeZoneId.Should().Be("UTC");
        appArgs.SecondsBetweenReports.Should().BeNull();
        appArgs.RetrySeconds.Should().BeNull();
        appArgs.Path.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnsSecondsBetweenReportsWhenItUseArgs()
    {
        // Act & Arrange
        AppArgs appArgs = new([AppArgs.SecondsBetweenReportsArg, "60"]);

        // Assert
        appArgs.SecondsBetweenReports.Should().Be(60);
        appArgs.TimeZoneId.Should().BeNull();
        appArgs.RetrySeconds.Should().BeNull();
        appArgs.Path.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnsSecondsBetweenReportsWhenItUseShortArgs()
    {
        // Act & Arrange
        AppArgs appArgs = new([AppArgs.SecondsBetweenReportsShortArg, "60"]);

        // Assert
        appArgs.SecondsBetweenReports.Should().Be(60);
        appArgs.TimeZoneId.Should().BeNull();
        appArgs.RetrySeconds.Should().BeNull();
        appArgs.Path.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnsRetrySecondsWhenItUseArgs()
    {
        // Arrange & Act
        AppArgs appArgs = new([AppArgs.RetrySecondsArg, "30"]);

        // Assert
        appArgs.RetrySeconds.Should().Be(30);
        appArgs.TimeZoneId.Should().BeNull();
        appArgs.SecondsBetweenReports.Should().BeNull();
        appArgs.Path.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnsRetrySecondsWhenItUseShortArgs()
    {
        // Arrange & Act
        AppArgs appArgs = new([AppArgs.RetrySecondsShortArg, "30"]);

        // Assert
        appArgs.RetrySeconds.Should().Be(30);
        appArgs.TimeZoneId.Should().BeNull();
        appArgs.SecondsBetweenReports.Should().BeNull();
        appArgs.Path.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnsPathWhenItUseArgs()
    {
        // Arrange & Act
        AppArgs appArgs = new([AppArgs.PathArg, "./reports"]);

        // Assert
        appArgs.Path.Should().Be("./reports");
        appArgs.TimeZoneId.Should().BeNull();
        appArgs.SecondsBetweenReports.Should().BeNull();
        appArgs.RetrySeconds.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnsPathWhenItUseShortArgs()
    {
        // Arrange & Act
        AppArgs appArgs = new([AppArgs.PathShortArg, "./reports"]);

        // Assert
        appArgs.Path.Should().Be("./reports");
        appArgs.TimeZoneId.Should().BeNull();
        appArgs.SecondsBetweenReports.Should().BeNull();
        appArgs.RetrySeconds.Should().BeNull();
    }
}
