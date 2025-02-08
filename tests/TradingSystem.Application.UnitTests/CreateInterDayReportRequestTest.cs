using FluentAssertions;

namespace TradingSystem.Application.UnitTests;

public class CreateInterDayReportRequestTest
{
    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Arrange
        var reportDate = new DateTime(2021, 1, 1);
        var timeZoneId = "Eastern Standard Time";

        // Act
        var createInterDayReportRequest = new CreateInterDayReportRequest(reportDate, timeZoneId);

        // Assert
        createInterDayReportRequest.ReportDate.Should().Be(reportDate);
        createInterDayReportRequest.TimeZone.Id.Should().Be(timeZoneId);
    }

    [Fact]
    public void ShouldCreateAnInstanceWithDefaultTimeZoneId()
    {
        // Arrange
        var reportDate = new DateTime(2021, 1, 1);
        var timeZoneId = "Central Europe Standard Time";

        // Act
        var createInterDayReportRequest = new CreateInterDayReportRequest(reportDate);

        // Assert
        createInterDayReportRequest.ReportDate.Should().Be(reportDate);
        createInterDayReportRequest.TimeZone.Id.Should().Be(timeZoneId);
    }

    [Fact]
    public void ShouldFailsWhenTimeZoneIdIsInvalid()
    {
        // Arrange
        var reportDate = new DateTime(2021, 1, 1);
        var timeZoneId = "fake-time-zone";

        // Act
        Action act = () => _ = new CreateInterDayReportRequest(reportDate, timeZoneId);

        // Assert
        act.Should().Throw<TimeZoneNotFoundException>();
    }

    [Fact]
    public void ShouldFailsWhenTimeZoneIdIsNull()
    {
        // Arrange
        DateTime reportDate = new(2021, 1, 1);
        string? timeZoneId = null!;

        // Act
        Action act = () => _ = new CreateInterDayReportRequest(reportDate, timeZoneId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Value cannot be null. (Parameter 'timeZoneId')");
    }

    [Fact]
    public void ShouldFailsWhenTimeZoneIdIsEmpty()
    {
        // Arrange
        DateTime reportDate = new(2021, 1, 1);
        string timeZoneId = ""!;

        // Act
        Action act = () => _ = new CreateInterDayReportRequest(reportDate, timeZoneId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'timeZoneId')");
    }
}
