using FluentAssertions;

namespace TradingSystem.Domain.UnitTests;

public class ReportPositionTest
{
    [Fact]
    public void ShouldCreateAnInstanceOfReportPositionWhenPassSlotAndVolume()
    {
        // Arrange
        DateTime slot = DateTime.UtcNow;
        double volume = 1.0;

        // Act
        ReportPosition reportPosition = new(slot, volume);

        // Assert
        reportPosition.Slot.Should().Be(slot);
        reportPosition.Volume.Should().Be(volume);
    }

    [Fact]
    public void ShouldBeEqualWhenComparingTwoReportPosition()
    {
        // Arrange
        DateTime slot = DateTime.UtcNow;
        ReportPosition reportPosition1 = new(slot, 1.0);
        ReportPosition reportPosition2 = new(slot, 2.0);

        // Act & Assert
        reportPosition1.Equals(reportPosition2).Should().BeTrue();
    }

    [Fact]
    public void ShouldNotBeEqualWhenComparingTwoReportPosition()
    {
        // Arrange
        DateTime slot1 = DateTime.UtcNow;
        DateTime slot2 = DateTime.UtcNow.AddDays(1);
        ReportPosition reportPosition1 = new(slot1, 1.0);
        ReportPosition reportPosition2 = new(slot2, 1.0);

        // Act & Assert
        reportPosition1.Equals(reportPosition2).Should().BeFalse();
    }

    [Fact]
    public void ShouldHaveSameHashCodeWhenComparingTwoReportPosition()
    {
        // Arrange
        DateTime slot = DateTime.UtcNow;
        ReportPosition reportPosition1 = new(slot, 1.0);
        ReportPosition reportPosition2 = new(slot, 2.0);

        // Act & Assert
        reportPosition1.GetHashCode().Should().Be(reportPosition2.GetHashCode());
    }

    [Fact]
    public void ShouldNotHaveSameHashCodeWhenComparingTwoReportPosition()
    {
        // Arrange
        DateTime slot1 = DateTime.UtcNow;
        DateTime slot2 = DateTime.UtcNow.AddDays(1);
        double volume = 1.0;
        ReportPosition reportPosition1 = new(slot1, volume);
        ReportPosition reportPosition2 = new(slot2, volume);

        // Act & Assert
        reportPosition1.GetHashCode().Should().NotBe(reportPosition2.GetHashCode());
    }
}
