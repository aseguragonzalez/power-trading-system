using FluentAssertions;
using NSubstitute;
using TradingSystem.Domain;
using TradingSystem.Domain.Services;

namespace TradingSystem.Application.UnitTests;

public class CreateInterDayReportTest
{
    private const string TimeZoneId = "Europe/Madrid";

    [Fact]
    public void ShouldFailsWhenReportRepositoryIsNull()
    {
        // Arrange
        ITradeService tradeService = Substitute.For<ITradeService>();
        IReportRepository? reportRepository = null!;

        // Act
        Action act = () => _ = new CreateInterDayReport(tradeService, reportRepository);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("reportRepository");

    }

    [Fact]
    public void ShouldFailsWhenTradeServiceIsNull()
    {
        // Arrange
        ITradeService? tradeService = null!;
        IReportRepository reportRepository = Substitute.For<IReportRepository>();

        // Act
        Action act = () => _ = new CreateInterDayReport(tradeService, reportRepository);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("tradeService");
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Arrange
        ITradeService tradeService = Substitute.For<ITradeService>();
        IReportRepository reportRepository = Substitute.For<IReportRepository>();

        // Act
        var createInterDayReport = new CreateInterDayReport(tradeService, reportRepository);

        // Assert
        createInterDayReport.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldSaveTheReportWhenExecuteIsCalled()
    {
        // Arrange
        ITradeService tradeService = Substitute.For<ITradeService>();
        IReportRepository reportRepository = Substitute.For<IReportRepository>();
        CreateInterDayReport createInterDayReport = new(tradeService, reportRepository);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
        DateTime createdAt = DateTime.UtcNow;
        DateTime date = createdAt.AddDays(1);
        tradeService.GetPositionsByDate(date).Returns(new TradePositions());
        CreateInterDayReportRequest createInterDayReportRequest = new(date, timeZoneInfo);

        // Act
        await createInterDayReport.Execute(createInterDayReportRequest);

        // Assert
        await reportRepository.Received().Save(Arg.Is<Report>(report => report.Date == date));
    }

    [Fact]
    public async Task ShouldApplyOffsetWhenUseCustomTimeZoneId()
    {
        // Arrange
        ITradeService tradeService = Substitute.For<ITradeService>();
        IReportRepository reportRepository = Substitute.For<IReportRepository>();
        CreateInterDayReport createInterDayReport = new(tradeService, reportRepository);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
        DateTime createdAt = DateTime.UtcNow;
        DateTime date = createdAt.AddDays(1);
        tradeService.GetPositionsByDate(date).Returns(new TradePositions());
        CreateInterDayReportRequest createInterDayReportRequest = new(date, timeZoneInfo);

        // Act
        await createInterDayReport.Execute(createInterDayReportRequest);

        // Assert
        await reportRepository.Received().Save(Arg.Is<Report>(report => report.Offset == timeZoneInfo.BaseUtcOffset.Hours));
    }
}
