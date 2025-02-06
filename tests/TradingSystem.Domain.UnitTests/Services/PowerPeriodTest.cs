namespace TradingSystem.Domain.UnitTests.Services;

using AutoFixture;
using FluentAssertions;
using TradingSystem.Domain.Services;

public class PowerPeriodTest
{
    const string outOfRangeErrorMessage = "PeriodId must be between 1 and 24 (Parameter 'periodId')";

    [Fact]
    public void ShouldCreateAnInstanceOfPowerPeriod()
    {
        // Arrange
        var fixture = new Fixture();
        int randomPeriodId = fixture.Create<int>() % 24 + 1;

        // Act
        PowerPeriod powerPeriod = new(periodId: randomPeriodId, volume: 1.0);

        // Assert
        powerPeriod.PeriodId.Should().Be(randomPeriodId);
        powerPeriod.Volume.Should().Be(1.0);
    }


    [Fact]
    public void ShouldFailWhenPeriodIsGreaterThanLast()
    {
        // Arrange
        var fixture = new Fixture();
        int periodId = fixture.Create<int>() + 25;
        Action act = () => _ = new PowerPeriod(periodId: periodId, volume: 1.0);

        // Act & Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage(outOfRangeErrorMessage);
    }

    [Fact]
    public void ShouldFailWhenPeriodIsLessThanFirst()
    {
        // Arrange
        var fixture = new Fixture();
        int periodId = fixture.Create<int>() % 24 - 24;
        Action act = () => _ = new PowerPeriod(periodId: periodId, volume: 1.0);

        // Act & Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage(outOfRangeErrorMessage);
    }
}
