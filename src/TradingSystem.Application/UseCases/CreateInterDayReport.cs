using Microsoft.Extensions.Logging;

using TradingSystem.Domain.Entities;
using TradingSystem.Domain.Repositories;
using TradingSystem.Domain.Services;

namespace TradingSystem.Application.UseCases;

public sealed class CreateInterDayReport : ICreateInterDayReport
{
    private readonly ITradeService tradeService;
    private readonly IReportRepository reportRepository;
    private readonly ILogger<CreateInterDayReport> logger;

    public CreateInterDayReport(ITradeService tradeService, IReportRepository reportRepository, ILogger<CreateInterDayReport> logger)
    {
        ArgumentNullException.ThrowIfNull(tradeService, nameof(tradeService));
        ArgumentNullException.ThrowIfNull(reportRepository, nameof(reportRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        this.tradeService = tradeService;
        this.reportRepository = reportRepository;
        this.logger = logger;
    }

    public async Task Execute(CreateInterDayReportRequest createInterDayReportRequest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(createInterDayReportRequest, nameof(createInterDayReportRequest));
        logger.LogInformation("Creating inter-day report for {ReportDate}", createInterDayReportRequest.ReportDate);
        TimeSpan offset = createInterDayReportRequest.TimeZone.GetUtcOffset(createInterDayReportRequest.ReportDate);
        TradePositions positions = await tradeService.GetPositionsByDate(createInterDayReportRequest.ReportDate, cancellationToken);
        Report report = new(createInterDayReportRequest.ReportDate, offset);
        report.AddTradePositions(positions);
        await reportRepository.Save(report, cancellationToken);
        logger.LogInformation("Inter-day report created for {ReportDate}", createInterDayReportRequest.ReportDate);
    }
}
