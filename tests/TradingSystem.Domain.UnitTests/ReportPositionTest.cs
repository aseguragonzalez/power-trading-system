using FluentAssertions;

namespace TradingSystem.Domain.UnitTests;

public class ReportPositionTest
{
    [Fact]
    public void ShouldCreateAnInstanceOfReportPositionWhenPassSlotAndVolume()
    {
        // Arrange
        DateTime period = DateTime.UtcNow;
        double volume = 1.0;

        // Act
        ReportPosition reportPosition = new(period, volume);

        // Assert
        reportPosition.Period.Should().Be(period);
        reportPosition.Volume.Should().Be(volume);
    }

    [Fact]
    public void ShouldBeEqualWhenComparingTwoReportPosition()
    {
        // Arrange
        DateTime period = DateTime.UtcNow;
        ReportPosition reportPosition1 = new(period, 1.0);
        ReportPosition reportPosition2 = new(period, 2.0);

        // Act & Assert
        reportPosition1.Equals(reportPosition2).Should().BeTrue();
    }

    [Fact]
    public void ShouldNotBeEqualWhenComparingTwoReportPosition()
    {
        // Arrange
        DateTime period1 = DateTime.UtcNow;
        DateTime period2 = DateTime.UtcNow.AddDays(1);
        ReportPosition reportPosition1 = new(period1, 1.0);
        ReportPosition reportPosition2 = new(period2, 1.0);

        // Act & Assert
        reportPosition1.Equals(reportPosition2).Should().BeFalse();
    }

    [Fact]
    public void ShouldHaveSameHashCodeWhenComparingTwoReportPosition()
    {
        // Arrange
        DateTime period = DateTime.UtcNow;
        ReportPosition reportPosition1 = new(period, 1.0);
        ReportPosition reportPosition2 = new(period, 2.0);

        // Act & Assert
        reportPosition1.GetHashCode().Should().Be(reportPosition2.GetHashCode());
    }

    [Fact]
    public void ShouldNotHaveSameHashCodeWhenComparingTwoReportPosition()
    {
        // Arrange
        DateTime period1 = DateTime.UtcNow;
        DateTime period2 = DateTime.UtcNow.AddDays(1);
        double volume = 1.0;
        ReportPosition reportPosition1 = new(period1, volume);
        ReportPosition reportPosition2 = new(period2, volume);

        // Act & Assert
        reportPosition1.GetHashCode().Should().NotBe(reportPosition2.GetHashCode());
    }
}
