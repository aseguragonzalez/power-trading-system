using TradingSystem.Domain;
using TradingSystem.Domain.Services;

namespace TradingSystem.Application;

public sealed class CreateInterDayReport
{
    private readonly ITradeService tradeService;
    private readonly IReportRepository reportRepository;

    public CreateInterDayReport(ITradeService tradeService, IReportRepository reportRepository)
    {
        ArgumentNullException.ThrowIfNull(tradeService, nameof(tradeService));
        ArgumentNullException.ThrowIfNull(reportRepository, nameof(reportRepository));
        this.tradeService = tradeService;
        this.reportRepository = reportRepository;
    }

    public async Task Execute(CreateInterDayReportRequest request)
    {
        TradePositions positions = await tradeService.GetPositionsByDate(request.ReportDate);
        Report report = new(request.ReportDate);
        report.AddTradePositions(positions);
        await reportRepository.Save(report);
    }
}
