using FluentAssertions;
using TradingSystem.Application.UseCases;

namespace TradingSystem.UnitTests.Application.UseCases;

public class CreateInterDayReportRequestTest
{
    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Arrange
        var reportDate = new DateTime(2021, 1, 1);
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Lisbon");

        // Act
        var createInterDayReportRequest = new CreateInterDayReportRequest(reportDate, timeZoneInfo);

        // Assert
        createInterDayReportRequest.ReportDate.Should().Be(reportDate);
        createInterDayReportRequest.TimeZone.Should().Be(timeZoneInfo);
    }

    [Fact]
    public void ShouldFailsWhenTimeZoneIsNull()
    {
        // Arrange
        DateTime reportDate = new(2021, 1, 1);

        // Act
        Action act = () => _ = new CreateInterDayReportRequest(reportDate, timeZoneInfo: null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'timeZoneInfo')");
    }
}
