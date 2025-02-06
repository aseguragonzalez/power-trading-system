using FluentAssertions;
using NSubstitute;
using TradingSystem.Domain;
using TradingSystem.Domain.Services;

namespace TradingSystem.Application.UnitTests;

public class CreateInterDayReportTest
{
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
        DateTime date = DateTime.UtcNow.AddDays(1);
        TradePositions tradePositions = new();
        tradeService.GetPositionsByDate(date).Returns(tradePositions);

        // Act
        await createInterDayReport.Execute(new CreateInterDayReportRequest(date));

        // Assert
        await reportRepository.Received().Save(Arg.Is<Report>(report => report.Date == date));
    }
}
