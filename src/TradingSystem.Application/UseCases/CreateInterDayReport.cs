using TradingSystem.Domain.Entities;
using TradingSystem.Domain.Repositories;
using TradingSystem.Domain.Services;

namespace TradingSystem.Application.UseCases;

public sealed class CreateInterDayReport : ICreateInterDayReport
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

    public async Task Execute(CreateInterDayReportRequest createInterDayReportRequest, CancellationToken cancellationToken = default)
    {
        TimeSpan offset = createInterDayReportRequest.TimeZone.GetUtcOffset(createInterDayReportRequest.ReportDate);
        TradePositions positions = await tradeService.GetPositionsByDate(createInterDayReportRequest.ReportDate, cancellationToken);
        Report report = new(createInterDayReportRequest.ReportDate, offset);
        report.AddTradePositions(positions);
        await reportRepository.Save(report, cancellationToken);
    }
}
