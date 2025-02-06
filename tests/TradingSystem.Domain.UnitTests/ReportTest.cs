using FluentAssertions;
using TradingSystem.Domain.Services;

namespace TradingSystem.Domain.UnitTests;

public class ReportTest
{
    [Fact]
    public void ShouldFailWhenAskingForPreviousDates()
    {
        // Arrange
        DateTime date = DateTime.UtcNow.AddDays(-1);
        Action act = () => _ = new Report(date: date);

        // Act & Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Date cannot be in the past. (Parameter 'date')");
    }

    [Fact]
    public void ShouldAnInstanceOfReportWhenPassCreateAtValue()
    {
        // Arrange
        DateTime createdAt = DateTime.UtcNow;
        DateTime date = createdAt.AddDays(1);

        // Act
        Report report = new(date: date, createdAt: createdAt);

        // Assert
        report.Date.Should().Be(date);
        report.CreatedAt.Should().Be(createdAt);
    }

    [Fact]
    public void ShouldAnInstanceOfDefaultReport()
    {
        // Arrange
        DateTime date = DateTime.UtcNow.AddDays(1);

        // Act
        Report report = new(date: date);

        // Assert
        report.Date.Should().Be(date);
        report.CreatedAt.Should().BeBefore(DateTime.UtcNow);
    }


    [Fact]
    public void ShouldRetrieveDefaultPositionsWhenNoTradePositionsWereAdded()
    {
        // Arrange
        Report report = new(DateTime.UtcNow.AddDays(1));

        // Act
        IEnumerable<ReportPosition> positions = report.GetPositions();

        // Assert
        positions.Should().HaveCount(24);
        positions.Should().NotContain(x => x.Volume != 0);
    }

    [Theory]
    [MemberData(nameof(GetTradePositionsTestData))]
    public void ShouldRetrieveCorrectPositionsWhenTradePositionsAreAdded(TradePositions tradePositions, IEnumerable<ReportPosition> expectedPositions)
    {
        // Arrange
        Report report = new(date: new DateTime(2023, 7, 2, 0, 0, 0), createdAt: new DateTime(2023, 7, 1, 19, 15, 0));
        report.AddTradePositions(tradePositions);

        // Act
        IEnumerable<ReportPosition> positions = report.GetPositions(timeOffset: 2);

        // Assert
        positions.Should().BeEquivalentTo(expectedPositions);
    }

    public static IEnumerable<object[]> GetTradePositionsTestData()
    {
        DateTime baseLine = new(2023, 7, 1, 22, 0, 0);
        yield return new object[]
        {
            new TradePositions(
                positions:
                [
                    new(1, 100),
                    new(2, 100),
                    new(3, 100),
                    new(4, 100),
                    new(5, 100),
                    new(6, 100),
                    new(7, 100),
                    new(8, 100),
                    new(9, 100),
                    new(10, 100),
                    new(11, 100),
                    new(12, 100),
                    new(13, 100),
                    new(14, 100),
                    new(15, 100),
                    new(16, 100),
                    new(17, 100),
                    new(18, 100),
                    new(19, 100),
                    new(20, 100),
                    new(21, 100),
                    new(22, 100),
                    new(23, 100),
                    new(24, 100),
                    new(1, 50),
                    new(2, 50),
                    new(3, 50),
                    new(4, 50),
                    new(5, 50),
                    new(6, 50),
                    new(7, 50),
                    new(8, 50),
                    new(9, 50),
                    new(10, 50),
                    new(11, 50),
                    new(12, -20),
                    new(13, -20),
                    new(14, -20),
                    new(15, -20),
                    new(16, -20),
                    new(17, -20),
                    new(18, -20),
                    new(19, -20),
                    new(20, -20),
                    new(21, -20),
                    new(22, -20),
                    new(23, -20),
                    new(24, -20),
                ]
            ),
            new ReportPosition[]
            {
                new(baseLine.AddHours(0), 150),
                new(baseLine.AddHours(1), 150),
                new(baseLine.AddHours(2), 150),
                new(baseLine.AddHours(3), 150),
                new(baseLine.AddHours(4), 150),
                new(baseLine.AddHours(5), 150),
                new(baseLine.AddHours(6), 150),
                new(baseLine.AddHours(7), 150),
                new(baseLine.AddHours(8), 150),
                new(baseLine.AddHours(9), 150),
                new(baseLine.AddHours(10), 150),
                new(baseLine.AddHours(11), 80),
                new(baseLine.AddHours(12), 80),
                new(baseLine.AddHours(13), 80),
                new(baseLine.AddHours(14), 80),
                new(baseLine.AddHours(15), 80),
                new(baseLine.AddHours(16), 80),
                new(baseLine.AddHours(17), 80),
                new(baseLine.AddHours(18), 80),
                new(baseLine.AddHours(19), 80),
                new(baseLine.AddHours(20), 80),
                new(baseLine.AddHours(21), 80),
                new(baseLine.AddHours(22), 80),
                new(baseLine.AddHours(23), 80),
            }
        };
    }
}
