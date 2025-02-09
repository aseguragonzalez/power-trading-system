using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TradingSystem.Application.UseCases;
using TradingSystem.Domain.Entities;
using TradingSystem.Domain.Repositories;
using TradingSystem.Domain.Services;

namespace TradingSystem.UnitTests.Application.UseCases;

public class CreateInterDayReportTest
{
    private const string TimeZoneId = "Europe/Madrid";

    [Fact]
    public void ShouldFailsWhenReportRepositoryIsNull()
    {
        // Arrange
        ILogger<CreateInterDayReport> logger = Substitute.For<ILogger<CreateInterDayReport>>();
        ITradeService tradeService = Substitute.For<ITradeService>();
        IReportRepository? reportRepository = null!;

        // Act
        Action act = () => _ = new CreateInterDayReport(tradeService, reportRepository, logger);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("reportRepository");
    }

    [Fact]
    public void ShouldFailsWhenLoggerIsNull()
    {
        // Arrange
        ILogger<CreateInterDayReport>? logger = null!;
        ITradeService tradeService = Substitute.For<ITradeService>();
        IReportRepository reportRepository = Substitute.For<IReportRepository>();

        // Act
        Action act = () => _ = new CreateInterDayReport(tradeService, reportRepository, logger);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("logger");
    }

    [Fact]
    public void ShouldFailsWhenTradeServiceIsNull()
    {
        // Arrange
        ILogger<CreateInterDayReport> logger = Substitute.For<ILogger<CreateInterDayReport>>();
        ITradeService? tradeService = null!;
        IReportRepository reportRepository = Substitute.For<IReportRepository>();

        // Act
        Action act = () => _ = new CreateInterDayReport(tradeService, reportRepository, logger);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("tradeService");
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Arrange
        ILogger<CreateInterDayReport> logger = Substitute.For<ILogger<CreateInterDayReport>>();
        ITradeService tradeService = Substitute.For<ITradeService>();
        IReportRepository reportRepository = Substitute.For<IReportRepository>();

        // Act
        var createInterDayReport = new CreateInterDayReport(tradeService, reportRepository, logger);

        // Assert
        createInterDayReport.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldSaveTheReportWhenExecuteIsCalled()
    {
        // Arrange
        ILogger<CreateInterDayReport> logger = Substitute.For<ILogger<CreateInterDayReport>>();
        ITradeService tradeService = Substitute.For<ITradeService>();
        IReportRepository reportRepository = Substitute.For<IReportRepository>();
        CreateInterDayReport createInterDayReport = new(tradeService, reportRepository, logger);
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
        ILogger<CreateInterDayReport> logger = Substitute.For<ILogger<CreateInterDayReport>>();
        ITradeService tradeService = Substitute.For<ITradeService>();
        IReportRepository reportRepository = Substitute.For<IReportRepository>();
        CreateInterDayReport createInterDayReport = new(tradeService, reportRepository, logger);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
        DateTime createdAt = DateTime.UtcNow;
        DateTime date = createdAt.AddDays(1);
        tradeService.GetPositionsByDate(date).Returns(new TradePositions());
        CreateInterDayReportRequest createInterDayReportRequest = new(date, timeZoneInfo);

        // Act
        await createInterDayReport.Execute(createInterDayReportRequest);

        // Assert
        await reportRepository.Received().Save(Arg.Is<Report>(report => report.Offset == timeZoneInfo.BaseUtcOffset));
    }
}
