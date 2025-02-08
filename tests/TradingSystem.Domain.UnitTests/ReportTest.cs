using FluentAssertions;
using TradingSystem.Domain.Services;

namespace TradingSystem.Domain.UnitTests;

public class ReportTest
{
    [Fact]
    public void ShouldFailWhenAskingForPreviousDates()
    {
        // Arrange
        DateTime createdAt = DateTime.UtcNow;
        DateTime date = createdAt.AddDays(-1);
        Action act = () => _ = new Report(createdAt: createdAt, date: date);

        // Act & Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ShouldCreateAnInstanceOfReport()
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
    public void ShouldCreateAnInstanceWithDateValue()
    {
        // Arrange
        DateTime date = DateTime.UtcNow.AddDays(1);

        // Act
        Report report = new(date: date, offset: 0);

        // Assert
        report.Date.Should().Be(date);
        report.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(10));
        report.Offset.Should().Be(0);
    }

    [Fact]
    public void ShouldEnsureDateIsInUtc()
    {
        // Arrange
        TimeZoneInfo berlinTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, berlinTimeZone);

        // Act
        Action act = () => _ = new Report(createdAt: DateTime.UtcNow, date: date.AddDays(1));

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Date must be in UTC. (Parameter 'date')");
    }

    [Fact]
    public void ShouldEnsureCreatedAtIsInUtc()
    {
        // Arrange
        TimeZoneInfo berlinTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        DateTime createdAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, berlinTimeZone);

        // Act
        Action act = () => _ = new Report(date: DateTime.UtcNow, createdAt: createdAt.AddDays(1));

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("CreatedAt must be in UTC. (Parameter 'createdAt')");
    }

    [Fact]
    public void ShouldRetrieveDefaultPositionsWhenNoTradePositionsWereAdded()
    {
        // Arrange
        Report report = new(DateTime.UtcNow.AddDays(1), offset: 0);

        // Act
        IEnumerable<ReportPosition> positions = report.GetPositions();

        // Assert
        positions.Should().HaveCount(24);
        positions.Should().NotContain(x => x.Volume != 0);
    }

    [Fact]
    public void ShouldRetrieveTheReportName()
    {
        // Arrange
        DateTime createdAt = new(2023, 7, 1, 19, 15, 0, DateTimeKind.Utc);
        Report report = new(date: createdAt.AddDays(1), createdAt: createdAt);

        // Act
        string reportName = report.ReportName;

        // Assert
        reportName.Should().Be("PowerPosition_20230702_202307011915.csv");
    }

    [Fact]
    public void ShouldFailsWhenAddingNullTradePositions()
    {
        // Arrange
        DateTime createdAt = new(2023, 7, 1, 19, 15, 0, DateTimeKind.Utc);
        Report report = new(date: createdAt.AddDays(1), createdAt: createdAt);

        // Act
        Action act = () => report.AddTradePositions(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'tradePositions')");
    }

    [Theory]
    [MemberData(nameof(GetTradePositionsTestData))]
    public void ShouldRetrieveCorrectPositionsWhenTradePositionsAreAdded(TradePositions tradePositions, IEnumerable<ReportPosition> expectedPositions)
    {
        // Arrange
        DateTime createdAt = new(2023, 7, 1, 19, 15, 0, DateTimeKind.Utc);
        Report report = new(date: createdAt.AddDays(1), createdAt: createdAt, offset: 2);
        report.AddTradePositions(tradePositions);

        // Act
        IEnumerable<ReportPosition> positions = report.GetPositions();

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
